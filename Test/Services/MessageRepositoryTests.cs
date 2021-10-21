using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using API.Services;
using AutoFixture;
using DataAccess;
using DataAccess.Entities;
using DataAccess.Enums;
using FluentAssertions;
using Test.TestHelpers;
using Xunit;

namespace Test.Services
{
    public class MessageRepositoryTests
    {
        private MessageRepository _sut;
        private DataContext _context;
        private Fixture _fixture;

        public MessageRepositoryTests()
        {
            _fixture = new Fixture();
            _fixture.Behaviors.Remove(new ThrowingRecursionBehavior());
            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());

            _context = DatabaseHelper.GetCleanDatabaseContext();
            _sut = new MessageRepository(_context);
        }

        [Fact]
        public async Task GetMessagesAsync_WhenDatabaseHasMessages_RetunsResultWithThem()
        {
            // Arrange
            var messages = _fixture.CreateMany<string>(15)
                .Select(str => new Message { Content = str, MessageType = MessageType.Primary })
                .ToList();

            _context.Messages.AddRange(messages);
            await _context.SaveChangesAsync();

            // Act
            var result = await _sut.GetMessagesAsync(0, 15);

            // Assert
            result.Data.Should()
                .BeEquivalentTo(messages);
        }
    }
}
