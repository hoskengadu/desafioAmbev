using DeveloperStore.Sales.Application.Abstractions;

namespace DeveloperStore.Sales.Infrastructure.Events;

public sealed class LoggerEventPublisher : IEventPublisher
{
    private readonly ILogger<LoggerEventPublisher> _logger;

    public LoggerEventPublisher(ILogger<LoggerEventPublisher> logger) => _logger = logger;

    public Task PublishAsync(IEnumerable<object> domainEvents, CancellationToken cancellationToken)
    {
        foreach (var domainEvent in domainEvents)
        {
            _logger.LogInformation("{EventName} {@Event}", domainEvent.GetType().Name, domainEvent);
        }

        return Task.CompletedTask;
    }
}
