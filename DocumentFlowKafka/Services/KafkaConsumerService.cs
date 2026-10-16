using Confluent.Kafka;
using DocumentFlowKafka.Model;
using DocumentFlowKafka.Classes;
using System.Text.Json;

namespace DocumentFlowKafka.Services
{
    public class KafkaConsumerService : BackgroundService
    {
        private readonly IConfiguration _config;
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<KafkaConsumerService> _logger;

        public KafkaConsumerService(
            IConfiguration config,
            IServiceProvider serviceProvider,
            ILogger<KafkaConsumerService> logger)
        {
            _config = config;
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var config = new ConsumerConfig
            {
                BootstrapServers = _config["Kafka:BootstrapServers"],
                GroupId = "documentflow-group",
                AutoOffsetReset = AutoOffsetReset.Earliest,
                EnableAutoCommit = false
            };

            using var consumer = new ConsumerBuilder<Null, string>(config).Build();
            consumer.Subscribe("flow-created");
            consumer.Subscribe("document-add-requested");

            _logger.LogInformation("Kafka Consumer запущен");

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    var result = consumer.Consume(stoppingToken);
                    _logger.LogInformation($"Получено сообщение: {result.Message.Value}");

                    // Обработка сообщения
                    await ProcessMessageAsync(
                        result.Topic,
                        result.Message.Value,
                        stoppingToken);

                    consumer.Commit(result);
                }
                catch (ConsumeException ex)
                {
                    _logger.LogError($"Ошибка Consumer: {ex.Error.Reason}");
                }
                catch (OperationCanceledException)
                {
                    break;
                }
            }

            consumer.Close();
        }

        /// <summary>
        /// Тут формируется файл документооборота и записывается в базу
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        private async Task ProcessMessageAsync(string topic, string json, CancellationToken cancellationToken)
        {
            using var scope = _serviceProvider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<DocumentFlowContext>();

            switch (topic)
            {
                case "document-add-requested":
                    //await HandleDocumentAddRequested(json, context, cancellationToken);
                    break;

                default:
                    _logger.LogWarning("Неизвестный топик: {Topic}", topic);
                    break;
            }
            var data = JsonSerializer.Deserialize<JsonElement>(json);
            _logger.LogInformation($"Обработка документа: {data.GetProperty("Id")}");

            await Task.CompletedTask;
        }

        #region Логика обработчика

        ///Осознал, как работает тут идёт обработка других микросервисов, пока API делает, что то свою не нужно перекдиться данными от сюда на верх так как просто целыми данные на верх физически без проблем не передам


        /// <summary>
        /// Класс для десиривлизации
        /// </summary>
        //public record AddDocumentCommand(
        //    string Id,
        //    string DocumentJson,
        //    int UserId,
        //    DateTime RequestedAt);

        //private async Task HandleDocumentAddRequested(
        //    string json,
        //    DocumentFlowContext context,
        //    CancellationToken cancellationToken)
        //{
        //    var command = JsonSerializer.Deserialize<AddDocumentCommand>(json);

        //    if (command is null)
        //    {
        //        _logger.LogWarning("Не удалось десериализовать команду document-add-requested");
        //        return;
        //    }

        //    var connector = await DocumnetFlowConnector.CreateAsync(
        //        context,
        //        command.DocumentJson,
        //        cancellationToken);

        //    _logger.LogInformation(
        //        "Документ {DocumentId} добавлен в документооборот {FlowId}",
        //        connector.Document.Id,
        //        connector.Flow.Id);
        //}
        #endregion
    }
}
