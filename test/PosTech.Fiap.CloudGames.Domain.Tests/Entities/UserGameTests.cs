using PosTech.Fiap.CloudGames.Domain.Entities;
using PosTech.Fiap.CloudGames.Domain.ValueObjects;
using FluentAssertions;

namespace PosTech.Fiap.CloudGames.Domain.Tests.Entities;

public class UserGameTests
{
    [Fact]
    public void Constructor_ShouldFillOwnershipData()
    {
        var userId = Guid.NewGuid();
        var gameId = Guid.NewGuid();

        var item = new UserGame(userId, gameId, Money.Create(59.90m));

        item.UserId.Should().Be(userId);
        item.GameId.Should().Be(gameId);
        item.PricePaid.Should().Be(59.90m);
    }

    [Fact]
    public void Constructor_ShouldGenerateDistinctIds()
    {
        var userId = Guid.NewGuid();
        var gameId = Guid.NewGuid();

        var first = new UserGame(userId, gameId, Money.Zero);
        var second = new UserGame(userId, gameId, Money.Zero);

        first.Id.Should().NotBeEmpty();
        first.Id.Should().NotBe(second.Id);
    }

    [Fact]
    public void Constructor_ShouldStampAcquisitionDateInUtc()
    {
        var before = DateTime.UtcNow;

        var item = new UserGame(Guid.NewGuid(), Guid.NewGuid(), Money.Create(10m));

        item.AcquiredAt.Kind.Should().Be(DateTimeKind.Utc);
        item.AcquiredAt.Should().BeOnOrAfter(before).And.BeOnOrBefore(DateTime.UtcNow);
    }

    [Fact]
    public void Constructor_WithFreeGame_ShouldKeepZeroPrice()
    {
        var item = new UserGame(Guid.NewGuid(), Guid.NewGuid(), Money.Zero);

        item.PricePaid.Should().Be(0m);
    }
}
