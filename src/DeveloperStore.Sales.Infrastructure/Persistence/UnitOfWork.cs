using DeveloperStore.Sales.Application.Abstractions;

namespace DeveloperStore.Sales.Infrastructure.Persistence;

public sealed class UnitOfWork : IUnitOfWork
{
    private readonly SalesDbContext _context;

    public UnitOfWork(SalesDbContext context) => _context = context;

    public Task<int> SaveChangesAsync(CancellationToken cancellationToken) => _context.SaveChangesAsync(cancellationToken);
}
