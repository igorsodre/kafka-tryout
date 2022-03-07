using System.Text.Json;
using System.Threading.Tasks;
using API.Domain;
using Confluent.Kafka;

namespace API.Kafka.Producers;

public class OrdersProducer : IEventProducer<Order>
{
    private readonly IProducer<string, string> _producer;

    public OrdersProducer(IProducer<string, string> producer)
    {
        _producer = producer;
    }

    public async Task<DefaultResult> ProduceAsync(Order product)
    {
        var message = JsonSerializer.Serialize(product);
        var result = await _producer.ProduceAsync(
            "orders",
            new Message<string, string>
            {
                Key = "place-order",
                Value = message
            }
        );

        return result.Status != PersistenceStatus.Persisted
            ? new DefaultResult
            {
                ErrorMessages = new[] { $"Failed to deliver message:({message}) {result.Status}" }
            }
            : new DefaultResult { Success = true };
    }
}