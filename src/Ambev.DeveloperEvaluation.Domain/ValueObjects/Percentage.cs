using Ambev.DeveloperEvaluation.Domain.Common;

namespace Ambev.DeveloperEvaluation.Domain.ValueObjects;

public sealed record Percentage
{
    public decimal Value { get; }

    public Percentage(decimal value)
    {
        if (value < 0 || value > 100)
        {
            throw new DomainException("Percentage must be between 0 and 100.");
        }

        Value = decimal.Round(value, 2, MidpointRounding.AwayFromZero);
    }

    public decimal ApplyTo(decimal amount) => decimal.Round(amount * Value / 100m, 2, MidpointRounding.AwayFromZero);
}

