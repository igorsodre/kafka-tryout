using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using API.Domain;
using Contracts.Requests;
using DataAccess.Entities;

namespace API.Services.Interfaces
{
    public interface IMessageRepository
    {
        public Task<DefaultResult<Message>> GetMessageAsync(string id, CancellationToken token = default);

        public Task<DefaultResult<IEnumerable<Message>>> GetMessagesAsync(
            uint? page,
            uint? numOfRows,
            CancellationToken token = default
        );

        public Task<DefaultResult> AddMessageAsync(IndexRequest content, CancellationToken token = default);
    }
}
