using Ambev.DeveloperEvaluation.Domain.Common;

namespace Ambev.DeveloperEvaluation.Domain.ValueObjects;

public sealed record Branch
{
    public Guid Id { get; }
    public string Name { get; }

    public Branch(Guid id, string name)
    {
        if (id == Guid.Empty) throw new DomainException("Branch id is required.");
        if (string.IsNullOrWhiteSpace(name)) throw new DomainException("Branch name is required.");
        Id = id;
        Name = name.Trim();
    }
}

