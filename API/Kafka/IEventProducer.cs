using System.Threading.Tasks;
using API.Domain;

namespace API.Kafka;

public interface IEventProducer<T>
{
    public Task<DefaultResult> ProduceAsync(T product);
}