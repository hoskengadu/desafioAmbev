namespace DeveloperStore.Sales.Domain.Common;

public abstract class Entity
{
    private readonly List<object> _domainEvents = [];

    public Guid Id { get; protected set; }
    public IReadOnlyCollection<object> DomainEvents => _domainEvents.AsReadOnly();

    protected void AddDomainEvent(object domainEvent) => _domainEvents.Add(domainEvent);
    public void ClearDomainEvents() => _domainEvents.Clear();
}
