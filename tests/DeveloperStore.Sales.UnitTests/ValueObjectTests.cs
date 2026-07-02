using DeveloperStore.Sales.Domain.Common;
using DeveloperStore.Sales.Domain.ValueObjects;
using FluentAssertions;

namespace DeveloperStore.Sales.UnitTests;

public sealed class ValueObjectTests
{
    [Fact]
    public void Money_should_round_to_two_decimals()
    {
        var money = new Money(10.235m);
        money.Amount.Should().Be(10.24m);
    }

    [Fact]
    public void SaleNumber_should_not_accept_empty_value()
    {
        Assert.Throws<DomainException>(() => new SaleNumber(string.Empty));
    }
}
