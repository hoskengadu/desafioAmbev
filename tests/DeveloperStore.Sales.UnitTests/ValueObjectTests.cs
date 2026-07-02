using DeveloperStore.Sales.Domain.Common;
using DeveloperStore.Sales.Domain.ValueObjects;

namespace DeveloperStore.Sales.UnitTests;

public sealed class ValueObjectTests
{
    [Fact]
    public void Money_should_round_to_two_decimals()
    {
        var money = new Money(10.235m);
        Assert.Equal(10.24m, money.Amount);
    }

    [Fact]
    public void SaleNumber_should_not_accept_empty_value()
    {
        Assert.Throws<DomainException>(() => new SaleNumber(string.Empty));
    }
}
