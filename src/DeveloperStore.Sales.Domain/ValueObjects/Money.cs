using DeveloperStore.Sales.Domain.Common;

namespace DeveloperStore.Sales.Domain.ValueObjects;

public sealed record Money
{
    public decimal Amount { get; }

    public Money(decimal amount)
    {
        if (amount < 0)
        {
            throw new DomainException("Money cannot be negative.");
        }

        Amount = decimal.Round(amount, 2, MidpointRounding.AwayFromZero);
    }

    public static Money Zero => new(0);

    public static Money operator +(Money left, Money right) => new(left.Amount + right.Amount);
    public static Money operator -(Money left, Money right) => new(left.Amount - right.Amount);
    public static Money operator *(Money money, decimal multiplier) => new(money.Amount * multiplier);

    public override string ToString() => Amount.ToString("0.00");
}
