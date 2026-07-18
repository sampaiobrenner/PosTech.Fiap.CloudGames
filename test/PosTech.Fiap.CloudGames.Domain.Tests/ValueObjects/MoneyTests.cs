using PosTech.Fiap.CloudGames.Domain.Exceptions;
using PosTech.Fiap.CloudGames.Domain.ValueObjects;
using FluentAssertions;

namespace PosTech.Fiap.CloudGames.Domain.Tests.ValueObjects;

public class MoneyTests
{
    [Fact]
    public void Create_WithPositiveValue_ShouldRoundToTwoDecimals()
    {
        var money = Money.Create(10.555m);

        money.Amount.Should().Be(10.56m);
        money.Currency.Should().Be("BRL");
    }

    [Fact]
    public void Create_WithNegativeValue_ShouldThrow()
    {
        var act = () => Money.Create(-1m);

        act.Should().Throw<DomainException>().WithMessage("*não pode ser negativo*");
    }

    [Theory]
    [InlineData(100, 10, 90)]
    [InlineData(100, 0, 100)]
    [InlineData(59.90, 50, 29.95)]
    public void ApplyDiscount_WithValidPercent_ShouldReduceAmount(decimal amount, decimal percent, decimal expected)
    {
        Money.Create(amount).ApplyDiscount(percent).Amount.Should().Be(expected);
    }

    [Theory]
    [InlineData(-5)]
    [InlineData(150)]
    public void ApplyDiscount_WithInvalidPercent_ShouldThrow(decimal percent)
    {
        var act = () => Money.Create(100m).ApplyDiscount(percent);

        act.Should().Throw<DomainException>().WithMessage("*entre 0 e 100*");
    }

    [Fact]
    public void Zero_ShouldBeZeroInDefaultCurrency()
    {
        Money.Zero.Amount.Should().Be(0m);
        Money.Zero.Currency.Should().Be(Money.DefaultCurrency);
    }

    [Theory]
    [InlineData("usd", "USD")]
    [InlineData("Eur", "EUR")]
    public void Create_WithLowercaseCurrency_ShouldNormalizeToUpperCase(string currency, string expected)
    {
        Money.Create(10m, currency).Currency.Should().Be(expected);
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    public void Create_WithBlankCurrency_ShouldThrow(string currency)
    {
        var act = () => Money.Create(10m, currency);

        act.Should().Throw<DomainException>().WithMessage("*moeda é obrigatória*");
    }

    [Fact]
    public void ApplyDiscount_WithFullPercent_ShouldReturnZero()
    {
        Money.Create(59.90m).ApplyDiscount(100).Amount.Should().Be(0m);
    }

    [Fact]
    public void Equals_WithSameAmountAndCurrency_ShouldBeTrue()
    {
        var money = Money.Create(19.99m);
        var other = Money.Create(19.99m);

        money.Equals(other).Should().BeTrue();
        money.GetHashCode().Should().Be(other.GetHashCode());
    }

    [Fact]
    public void Equals_WithDifferentCurrency_ShouldBeFalse()
    {
        Money.Create(19.99m, "BRL").Equals(Money.Create(19.99m, "USD")).Should().BeFalse();
    }
}
