using DeveloperStore.Sales.Application.Abstractions;

namespace DeveloperStore.Sales.Infrastructure.Persistence;

public sealed class InMemoryUnitOfWork : IUnitOfWork
{
    public Task<int> SaveChangesAsync(CancellationToken cancellationToken) => Task.FromResult(0);
}
