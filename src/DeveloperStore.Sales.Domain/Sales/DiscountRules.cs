using DeveloperStore.Sales.Domain.Common;
using DeveloperStore.Sales.Domain.ValueObjects;

namespace DeveloperStore.Sales.Domain.Sales;

public static class DiscountRules
{
    public static Percentage Resolve(int quantity) => quantity switch
    {
        < 1 => throw new DomainException("Quantity must be greater than zero."),
        < 4 => new Percentage(0),
        <= 9 => new Percentage(10),
        <= 20 => new Percentage(20),
        _ => throw new DomainException("Quantity above 20 units is not allowed.")
    };
}
