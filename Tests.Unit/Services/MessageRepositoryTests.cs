using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Domain;
using API.Services;
using AutoFixture;
using DataAccess.Entities;
using DataAccess.Enums;
using FluentAssertions;
using Test.Unit;
using Tests.Unit.Helpers;
using Xunit;

namespace Tests.Unit.Services;

public class MessageRepositoryTests : UnitTestBase
{
    private readonly MessageRepository _sut;

    public MessageRepositoryTests(DbContextFixture contextFixture) : base(contextFixture)
    {
        _sut = new MessageRepository(Context);
    }


    [Fact]
    public async Task GetMessagesAsync_WhenDatabaseHasMessages_RetunsResultWithThem_BaseCase()
    {
        // Arrange
        var messages = Fixture.Build<Message>()
            .With(x => x.MessageType, MessageType.Primary)
            .With(x => x.MainMessageId, () => null)
            .With(x => x.Replies, new List<Message>())
            .CreateMany(30)
            .ToList();

        Context.Messages.AddRange(messages);
        await Context.SaveChangesAsync();


        // Act
        var result = await _sut.GetMessagesAsync(new PaginationFilter { PageNumber = 1, PageSize = 15 });

        // Assert
        result.Should()
            .BeEquivalentTo(messages.OrderByDescending(x => x.CreatedAt).Take(15));
    }
        
    [Fact]
    public async Task GetMessagesAsync_WhenDatabaseHasMessages_RetunsResultWithThem_PaginatedCase()
    {
        // Arrange
        var messages = Fixture.Build<Message>()
            .With(x => x.MessageType, MessageType.Primary)
            .With(x => x.MainMessageId, () => null)
            .With(x => x.Replies, new List<Message>())
            .CreateMany(45)
            .ToList();

        Context.Messages.AddRange(messages);
        await Context.SaveChangesAsync();


        // Act
        var result = await _sut.GetMessagesAsync(new PaginationFilter { PageNumber = 3, PageSize = 15 });

        // Assert
        result.Should()
            .BeEquivalentTo(messages.OrderByDescending(x => x.CreatedAt).Skip(30).Take(15));
    }
}