using DeveloperStore.Sales.Application.Abstractions;
using DeveloperStore.Sales.Domain.Sales;
using Microsoft.EntityFrameworkCore;

namespace DeveloperStore.Sales.Infrastructure.Persistence;

public sealed class SaleRepository : ISaleRepository
{
    private readonly SalesDbContext _context;

    public SaleRepository(SalesDbContext context) => _context = context;

    public Task AddAsync(Sale sale, CancellationToken cancellationToken)
        => _context.Sales.AddAsync(sale, cancellationToken).AsTask();

    public Task<Sale?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        => _context.Sales.Include(x => x.SaleItems).FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

    public Task<Sale?> GetByNumberAsync(string saleNumber, CancellationToken cancellationToken)
        => _context.Sales.Include(x => x.SaleItems).FirstOrDefaultAsync(x => x.SaleNumber.Value == saleNumber, cancellationToken);

    public async Task<IReadOnlyList<Sale>> GetAllAsync(CancellationToken cancellationToken)
        => await _context.Sales.Include(x => x.SaleItems).ToListAsync(cancellationToken);

    public void Update(Sale sale) => _context.Sales.Update(sale);

    public void Remove(Sale sale) => _context.Sales.Remove(sale);
}
