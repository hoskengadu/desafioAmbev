using DeveloperStore.Sales.Application.Abstractions;
using DeveloperStore.Sales.Domain.Sales;
using Microsoft.EntityFrameworkCore;

namespace DeveloperStore.Sales.Infrastructure.Persistence;

public sealed class SalesDbContext : DbContext, IApplicationDbContext, IUnitOfWork
{
    public SalesDbContext(DbContextOptions<SalesDbContext> options) : base(options)
    {
    }

    public DbSet<Sale> Sales => Set<Sale>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(SalesDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }
}
