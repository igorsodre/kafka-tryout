using System.Text.Json;
using System.Threading;
using API.Domain;
using Confluent.Kafka;

namespace API.Kafka.Consumers;

public class OrdersConsumer : IEventConsumer<Order>
{
    private readonly IConsumer<string, string> _consumer;
    private ConsumeResult<string, string> _result;

    public OrdersConsumer(IConsumer<string, string> consumer)
    {
        _consumer = consumer;
        _consumer.Subscribe("orders");
    }

    public Order Consume(CancellationToken cancellationToken = default)
    {
        try
        {
            _result = _consumer.Consume(cancellationToken);
            return _result is null ? null : JsonSerializer.Deserialize<Order>(_result.Message.Value);
        }
        catch
        {
            return null;
        }
    }

    public void Commit()
    {
        _consumer.Commit(_result);
    }

    public Order ConsumeAndCommit(CancellationToken cancellationToken = default)
    {
        var value = Consume(cancellationToken);
        Commit();
        return value;
    }
}
