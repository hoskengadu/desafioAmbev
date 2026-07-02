using DeveloperStore.Sales.Domain.Sales;
using Microsoft.EntityFrameworkCore;

namespace DeveloperStore.Sales.Application.Abstractions;

public interface IApplicationDbContext
{
    DbSet<Sale> Sales { get; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
