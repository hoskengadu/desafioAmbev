using DeveloperStore.Sales.Domain.Sales;

namespace DeveloperStore.Sales.Infrastructure.Persistence;

public sealed class InMemorySaleStore
{
    private readonly List<Sale> _sales = [];
    public IReadOnlyCollection<Sale> Sales => _sales.AsReadOnly();
    public void Add(Sale sale) => _sales.Add(sale);
    public void Remove(Sale sale) => _sales.Remove(sale);
}
