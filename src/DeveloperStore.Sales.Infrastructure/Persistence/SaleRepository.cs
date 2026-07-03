using DeveloperStore.Sales.Application.Abstractions;
using DeveloperStore.Sales.Application.Sales;
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

    public async Task<(IReadOnlyList<Sale> Items, int TotalCount)> SearchAsync(SaleSearchRequest request, CancellationToken cancellationToken)
    {
        var query = BuildQuery(request);
        var totalCount = await query.CountAsync(cancellationToken);

        var pageNumber = Math.Max(request.PageNumber, 1);
        var pageSize = Math.Clamp(request.PageSize, 1, 100);

        var items = await query
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return (items, totalCount);
    }

    public async Task<IReadOnlyList<Sale>> GetAllAsync(CancellationToken cancellationToken)
        => await _context.Sales.Include(x => x.SaleItems).ToListAsync(cancellationToken);

    public void Update(Sale sale) => _context.Sales.Update(sale);

    public void Remove(Sale sale) => _context.Sales.Remove(sale);

    private IQueryable<Sale> BuildQuery(SaleSearchRequest request)
    {
        var query = _context.Sales
            .AsNoTracking()
            .Include(x => x.SaleItems)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(request.SaleNumber))
        {
            query = query.Where(x => x.SaleNumber.Value.Contains(request.SaleNumber.Trim()));
        }

        if (!string.IsNullOrWhiteSpace(request.CustomerName))
        {
            query = query.Where(x => x.Customer.Name.Contains(request.CustomerName.Trim()));
        }

        if (!string.IsNullOrWhiteSpace(request.BranchName))
        {
            query = query.Where(x => x.Branch.Name.Contains(request.BranchName.Trim()));
        }

        if (request.Cancelled.HasValue)
        {
            query = query.Where(x => x.Cancelled == request.Cancelled.Value);
        }

        if (request.SaleDateFrom.HasValue)
        {
            query = query.Where(x => x.SaleDate >= request.SaleDateFrom.Value);
        }

        if (request.SaleDateTo.HasValue)
        {
            query = query.Where(x => x.SaleDate <= request.SaleDateTo.Value);
        }

        query = request.SortBy switch
        {
            SaleSortField.SaleNumber => request.SortDirection == SortOrder.Asc
                ? query.OrderBy(x => x.SaleNumber.Value).ThenBy(x => x.Id)
                : query.OrderByDescending(x => x.SaleNumber.Value).ThenByDescending(x => x.Id),
            SaleSortField.CustomerName => request.SortDirection == SortOrder.Asc
                ? query.OrderBy(x => x.Customer.Name).ThenBy(x => x.Id)
                : query.OrderByDescending(x => x.Customer.Name).ThenByDescending(x => x.Id),
            SaleSortField.BranchName => request.SortDirection == SortOrder.Asc
                ? query.OrderBy(x => x.Branch.Name).ThenBy(x => x.Id)
                : query.OrderByDescending(x => x.Branch.Name).ThenByDescending(x => x.Id),
            SaleSortField.TotalAmount => request.SortDirection == SortOrder.Asc
                ? query.OrderBy(x => x.TotalAmount.Amount).ThenBy(x => x.Id)
                : query.OrderByDescending(x => x.TotalAmount.Amount).ThenByDescending(x => x.Id),
            SaleSortField.CreatedAt => request.SortDirection == SortOrder.Asc
                ? query.OrderBy(x => x.CreatedAt).ThenBy(x => x.Id)
                : query.OrderByDescending(x => x.CreatedAt).ThenByDescending(x => x.Id),
            _ => request.SortDirection == SortOrder.Asc
                ? query.OrderBy(x => x.SaleDate).ThenBy(x => x.Id)
                : query.OrderByDescending(x => x.SaleDate).ThenByDescending(x => x.Id)
        };

        return query;
    }
}
