using Ambev.DeveloperEvaluation.Domain.Common;

namespace Ambev.DeveloperEvaluation.Domain.ValueObjects;

public sealed record SaleNumber
{
    public string Value { get; }

    public SaleNumber(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new DomainException("Sale number cannot be empty.");
        }

        Value = value.Trim();
    }

    public override string ToString() => Value;
}

