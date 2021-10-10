using System.Threading;
using System.Threading.Tasks;
using API.Domain;

namespace API.Kafka
{
    public interface IEventConsumer<T>
    {
        public T Consume(CancellationToken cancellationToken = default);
        public void Commit();
        public T ConsumeAndCommit(CancellationToken cancellationToken = default);
    }
}
