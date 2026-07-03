namespace Ambev.DeveloperEvaluation.Application.Abstractions;

public interface IEventPublisher
{
    Task PublishAsync(IEnumerable<object> domainEvents, CancellationToken cancellationToken);
}

