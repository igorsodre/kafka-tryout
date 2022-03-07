using AutoFixture;
using DataAccess;
using Tests.Unit.Helpers;
using Xunit;

namespace Test.Unit;

[Collection("NeedsSameDbContext")]
public class UnitTestBase : IClassFixture<DbContextFixture>
{
    protected DataContext Context { get; }
    protected Fixture Fixture;

    public UnitTestBase(DbContextFixture contextFixture)
    {
        Context = contextFixture.Context;
        Fixture = new Fixture();
        Fixture.Behaviors.Remove(new ThrowingRecursionBehavior());
        Fixture.Behaviors.Add(new OmitOnRecursionBehavior());
    }
}