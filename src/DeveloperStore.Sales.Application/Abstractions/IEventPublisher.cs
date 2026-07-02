namespace DeveloperStore.Sales.Application.Abstractions;

public interface IEventPublisher
{
    Task PublishAsync(IEnumerable<object> domainEvents, CancellationToken cancellationToken);
}
