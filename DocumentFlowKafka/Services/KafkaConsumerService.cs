using Confluent.Kafka;
using DocumentFlowKafka.Model;
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
            consumer.Subscribe("document-created");

            _logger.LogInformation("Kafka Consumer запущен");

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    var result = consumer.Consume(stoppingToken);
                    _logger.LogInformation($"Получено сообщение: {result.Message.Value}");

                    // Обработка сообщения
                    await ProcessMessageAsync(result.Message.Value);

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

        private async Task ProcessMessageAsync(string json)
        {
            using var scope = _serviceProvider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<DocumentFlowContext>();

            var data = JsonSerializer.Deserialize<JsonElement>(json);
            _logger.LogInformation($"Обработка документа: {data.GetProperty("Id")}");

            await Task.CompletedTask;
        }
    }
}
