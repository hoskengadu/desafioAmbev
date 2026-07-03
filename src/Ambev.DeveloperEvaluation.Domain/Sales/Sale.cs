using Ambev.DeveloperEvaluation.Domain.Common;
using Ambev.DeveloperEvaluation.Domain.ValueObjects;

namespace Ambev.DeveloperEvaluation.Domain.Sales;

public sealed class Sale : AggregateRoot
{
    private readonly List<SaleItem> _items = [];

    public SaleNumber SaleNumber { get; private set; } = null!;
    public DateTime SaleDate { get; private set; }
    public Customer Customer { get; private set; } = null!;
    public Branch Branch { get; private set; } = null!;
    public Money TotalAmount { get; private set; } = Money.Zero;
    public bool Cancelled { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime? UpdatedAt { get; private set; }
    public IReadOnlyCollection<SaleItem> SaleItems => _items.AsReadOnly();

    private Sale()
    {
    }

    public Sale(SaleNumber saleNumber, DateTime saleDate, Customer customer, Branch branch, IEnumerable<SaleItem> items)
    {
        Id = Guid.NewGuid();
        SaleNumber = saleNumber;
        SaleDate = saleDate;
        Customer = customer;
        Branch = branch;
        CreatedAt = DateTime.UtcNow;
        UpdateItems(items);
        RecalculateTotal();
        AddDomainEvent(new SaleCreatedEvent(Id));
    }

    public void Update(SaleNumber saleNumber, DateTime saleDate, Customer customer, Branch branch, IEnumerable<SaleItem> items)
    {
        EnsureNotCancelled();
        SaleNumber = saleNumber;
        SaleDate = saleDate;
        Customer = customer;
        Branch = branch;
        UpdateItems(items);
        UpdatedAt = DateTime.UtcNow;
        RecalculateTotal();
        AddDomainEvent(new SaleModifiedEvent(Id));
    }

    public void Cancel()
    {
        EnsureNotCancelled();
        Cancelled = true;
        UpdatedAt = DateTime.UtcNow;
        AddDomainEvent(new SaleCancelledEvent(Id));
    }

    public void CancelItem(Guid itemId)
    {
        EnsureNotCancelled();
        var item = _items.FirstOrDefault(x => x.Id == itemId) ?? throw new DomainException("Sale item not found.");
        item.Cancel();
        UpdatedAt = DateTime.UtcNow;
        RecalculateTotal();
        AddDomainEvent(new ItemCancelledEvent(Id, itemId));
    }

    private void UpdateItems(IEnumerable<SaleItem> items)
    {
        _items.Clear();
        _items.AddRange(items);
        if (_items.Count == 0) throw new DomainException("Sale must contain at least one item.");
    }

    private void RecalculateTotal() => TotalAmount = new Money(_items.Where(x => !x.Cancelled).Sum(x => x.Total.Amount));

    private void EnsureNotCancelled()
    {
        if (Cancelled) throw new DomainException("Sale is already cancelled.");
    }
}

