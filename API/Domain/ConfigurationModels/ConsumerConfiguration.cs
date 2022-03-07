using Confluent.Kafka;

namespace API.Domain.ConfigurationModels;

public class ConsumerConfiguration
{
    public bool EnableAutoCommit { get; set; }
    public bool EnableAutoOffsetStore { get; set; }
    public bool EnablePartitionEof { get; set; }
    public string BootstrapServers { get; set; }
    public string GroupId { get; set; }
    public int MaxPollIntervalMs { get; set; }
    public int SessionTimeoutMs { get; set; }
    public AutoOffsetReset AutoOffsetReset { get; set; }
}