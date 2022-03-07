using System.Threading;

namespace API.Kafka;

public interface IEventConsumer<T>
{
    public T Consume(CancellationToken cancellationToken = default);
    public void Commit();
    public T ConsumeAndCommit(CancellationToken cancellationToken = default);
}