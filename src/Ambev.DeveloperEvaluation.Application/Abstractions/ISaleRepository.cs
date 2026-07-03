using Ambev.DeveloperEvaluation.Application.Sales;
using Ambev.DeveloperEvaluation.Domain.Sales;

namespace Ambev.DeveloperEvaluation.Application.Abstractions;

public interface ISaleRepository
{
    Task AddAsync(Sale sale, CancellationToken cancellationToken);
    Task<Sale?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    Task<Sale?> GetByNumberAsync(string saleNumber, CancellationToken cancellationToken);
    Task<(IReadOnlyList<Sale> Items, int TotalCount)> SearchAsync(SaleSearchRequest request, CancellationToken cancellationToken);
    Task<IReadOnlyList<Sale>> GetAllAsync(CancellationToken cancellationToken);
    void Update(Sale sale);
    void Remove(Sale sale);
}

