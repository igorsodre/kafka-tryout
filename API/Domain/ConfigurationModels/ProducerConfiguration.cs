using Confluent.Kafka;

namespace API.Domain.ConfigurationModels
{
    public class ProducerConfiguration
    {
        public string BootstrapServers { get; set; }
        public int MessageSendMaxRetries { get; set; }
        public int RetryBackoffMs { get; set; }
        public bool EnableIdempotence { get; set; }

        public Acks Acks { get; set; }
    }
}
