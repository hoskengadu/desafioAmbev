using DeveloperStore.Sales.Application.Abstractions;
using DeveloperStore.Sales.Domain.Sales;

namespace DeveloperStore.Sales.Infrastructure.Persistence;

public sealed class InMemorySaleRepository : ISaleRepository
{
    private readonly InMemorySaleStore _store;

    public InMemorySaleRepository(InMemorySaleStore store) => _store = store;

    public Task AddAsync(Sale sale, CancellationToken cancellationToken)
    {
        _store.Add(sale);
        return Task.CompletedTask;
    }

    public Task<Sale?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        => Task.FromResult(_store.Sales.FirstOrDefault(x => x.Id == id));

    public Task<Sale?> GetByNumberAsync(string saleNumber, CancellationToken cancellationToken)
        => Task.FromResult(_store.Sales.FirstOrDefault(x => x.SaleNumber.Value == saleNumber));

    public Task<IReadOnlyList<Sale>> GetAllAsync(CancellationToken cancellationToken)
        => Task.FromResult((IReadOnlyList<Sale>)_store.Sales.ToList());

    public void Update(Sale sale)
    {
    }

    public void Remove(Sale sale) => _store.Remove(sale);
}
