using Ambev.DeveloperEvaluation.Application.Abstractions;
using Microsoft.Extensions.Logging;

namespace Ambev.DeveloperEvaluation.ORM.Events;

public sealed class LoggerEventPublisher : IEventPublisher
{
    private readonly ILogger<LoggerEventPublisher> _logger;

    public LoggerEventPublisher(ILogger<LoggerEventPublisher> logger) => _logger = logger;

    public Task PublishAsync(IEnumerable<object> domainEvents, CancellationToken cancellationToken)
    {
        foreach (var domainEvent in domainEvents)
        {
            var eventName = domainEvent.GetType().Name.Replace("Event", string.Empty, StringComparison.Ordinal);
            _logger.LogInformation("{EventName} {@Event}", eventName, domainEvent);
        }

        return Task.CompletedTask;
    }
}

