using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using API.Domain;
using API.Services.Interfaces;
using Contracts.Enums;
using Contracts.Requests;
using DataAccess;
using DataAccess.Entities;
using DataAccess.Enums;
using Microsoft.EntityFrameworkCore;

namespace API.Services
{
    public class MessageRepository : IMessageRepository
    {
        private readonly DataContext _context;
        private const int MaxPageSize = 30;

        public MessageRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<DefaultResult<Message>> GetMessageAsync(
            string id,
            CancellationToken token = default
        )
        {
            var requestContent = await _context.Messages.FindAsync(new object[] { id }, token);
            return new DefaultResult<Message>(requestContent);
        }


        public async Task<DefaultResult<IEnumerable<Message>>> GetMessagesAsync(
            uint? page,
            uint? numOfRows,
            CancellationToken token = default
        )
        {
            var twoDaysAgo = DateTime.Now.AddMinutes(-20);
            var numOfResults = Math.Min(numOfRows ?? 15, MaxPageSize);
            var requestContents = await _context.Messages
                .Where(r => r.CreatedAt > twoDaysAgo && r.MessageType == MessageType.Primary)
                .Include(r => r.Replies)
                .OrderByDescending(r => r.CreatedAt)
                .Skip((int)(numOfResults * (page ?? 0)))
                .Take((int)numOfResults)
                .ToListAsync(token);

            return new DefaultResult<IEnumerable<Message>>(requestContents);
        }

        public async Task<DefaultResult> AddMessageAsync(IndexRequest content, CancellationToken token = default)
        {
            var message = new Message()
            {
                Content = content.Content,
                MessageType = (MessageType)content.Type,
                MainMessageId = content.RepliedToId
            };

            _context.Messages.Add(message);
            await _context.SaveChangesAsync(token);
            return new DefaultResult { Success = true };
        }
    }
}
