using Ambev.DeveloperEvaluation.Domain.Common;

namespace Ambev.DeveloperEvaluation.Domain.ValueObjects;

public sealed record Customer
{
    public Guid Id { get; }
    public string Name { get; }

    public Customer(Guid id, string name)
    {
        if (id == Guid.Empty) throw new DomainException("Customer id is required.");
        if (string.IsNullOrWhiteSpace(name)) throw new DomainException("Customer name is required.");
        Id = id;
        Name = name.Trim();
    }
}

