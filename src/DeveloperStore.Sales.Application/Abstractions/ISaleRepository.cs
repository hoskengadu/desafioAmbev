using DeveloperStore.Sales.Domain.Sales;

namespace DeveloperStore.Sales.Application.Abstractions;

public interface ISaleRepository
{
    Task AddAsync(Sale sale, CancellationToken cancellationToken);
    Task<Sale?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    Task<Sale?> GetByNumberAsync(string saleNumber, CancellationToken cancellationToken);
    Task<IReadOnlyList<Sale>> GetAllAsync(CancellationToken cancellationToken);
    void Update(Sale sale);
    void Remove(Sale sale);
}
