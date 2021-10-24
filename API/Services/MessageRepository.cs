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


        public async Task<IList<Message>> GetMessagesAsync(
            PaginationFilter filter,
            CancellationToken token = default
        )
        {
            var skip = (filter.PageNumber - 1) * filter.PageSize;
            return await _context.Messages
                .Where(r => r.MessageType == MessageType.Primary)
                .Include(r => r.Replies)
                .OrderByDescending(r => r.CreatedAt)
                .Skip(skip)
                .Take(filter.PageSize)
                .ToListAsync(token);
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
