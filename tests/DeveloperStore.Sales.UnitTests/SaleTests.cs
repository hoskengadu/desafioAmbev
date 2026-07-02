using DeveloperStore.Sales.Domain.Common;
using DeveloperStore.Sales.Domain.Sales;
using DeveloperStore.Sales.Domain.ValueObjects;

namespace DeveloperStore.Sales.UnitTests;

public sealed class SaleTests
{
    [Fact]
    public void Sale_should_recalculate_total_and_raise_event()
    {
        var sale = new Sale(
            new SaleNumber("1"),
            DateTime.UtcNow,
            new Customer(Guid.NewGuid(), "Customer"),
            new Branch(Guid.NewGuid(), "Branch"),
            [
                new SaleItem(Guid.NewGuid(), "Product", 4, new Money(10))
            ]);

        Assert.Equal(36, sale.TotalAmount.Amount);
        Assert.Contains(sale.DomainEvents, e => e is SaleCreatedEvent);
    }

    [Fact]
    public void Sale_should_throw_when_cancel_item_not_found()
    {
        var sale = new Sale(
            new SaleNumber("1"),
            DateTime.UtcNow,
            new Customer(Guid.NewGuid(), "Customer"),
            new Branch(Guid.NewGuid(), "Branch"),
            [new SaleItem(Guid.NewGuid(), "Product", 4, new Money(10))]);

        Assert.Throws<DomainException>(() => sale.CancelItem(Guid.NewGuid()));
    }
}
