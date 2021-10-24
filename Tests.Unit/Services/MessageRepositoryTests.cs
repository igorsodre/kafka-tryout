using System.Linq;
using System.Threading.Tasks;
using API.Domain;
using API.Services;
using AutoFixture;
using DataAccess;
using DataAccess.Entities;
using DataAccess.Enums;
using FluentAssertions;
using Test.Unit;
using Tests.Unit.Helpers;
using Xunit;

namespace Tests.Unit.Services
{
    public class MessageRepositoryTests : UnitTestBase
    {
        private readonly MessageRepository _sut;

        public MessageRepositoryTests(DbContextFixture contextFixture) : base(contextFixture)
        {
            _sut = new MessageRepository(Context);
        }


        [Fact]
        public async Task GetMessagesAsync_WhenDatabaseHasMessages_RetunsResultWithThem()
        {
            // Arrange
            var messages = Fixture.CreateMany<string>(15)
                .Select(str => new Message { Content = str, MessageType = MessageType.Primary })
                .ToList();

            Context.Messages.AddRange(messages);
            await Context.SaveChangesAsync();


            // Act
            var result = await _sut.GetMessagesAsync(new PaginationFilter { PageNumber = 1, PageSize = 15 });

            // Assert
            result.Should()
                .BeEquivalentTo(messages);
        }
    }
}
