using Confluent.Kafka;
using System.Text.Json;

namespace DocumentFlowKafka.Services
{
    public class KafkaProducerService
    {
        private readonly IProducer<Null, string> _producer;
        private readonly ILogger<KafkaProducerService> _logger;

        public KafkaProducerService(IProducer<Null, string> producer, ILogger<KafkaProducerService> logger)
        {
            _producer = producer;
            _logger = logger;
        }

        public async Task SendAsync<T>(string topic, T message)
        {
            try
            {
                var json = JsonSerializer.Serialize(message);
                var result = await _producer.ProduceAsync(topic, new Message<Null, string>
                {
                    Value = json
                });

                _logger.LogInformation($"Сообщение отправлено в {topic}: {result.Offset}");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ошибка отправки в Kafka: {ex.Message}");
                throw;
            }
        }
    }
}
