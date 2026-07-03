using Ambev.DeveloperEvaluation.Domain.Common;
using Ambev.DeveloperEvaluation.Domain.ValueObjects;

namespace Ambev.DeveloperEvaluation.Domain.Sales;

public sealed class SaleItem : Entity
{
    public Guid ProductId { get; private set; }
    public string ProductName { get; private set; } = string.Empty;
    public int Quantity { get; private set; }
    public Money UnitPrice { get; private set; } = Money.Zero;
    public Percentage DiscountPercentage { get; private set; } = new(0);
    public Money DiscountAmount { get; private set; } = Money.Zero;
    public Money Total { get; private set; } = Money.Zero;
    public bool Cancelled { get; private set; }

    private SaleItem()
    {
    }

    public SaleItem(Guid productId, string productName, int quantity, Money unitPrice)
    {
        Id = Guid.NewGuid();
        if (productId == Guid.Empty) throw new DomainException("Product id is required.");
        if (string.IsNullOrWhiteSpace(productName)) throw new DomainException("Product name is required.");
        if (quantity <= 0) throw new DomainException("Quantity must be greater than zero.");

        ProductId = productId;
        ProductName = productName.Trim();
        Quantity = quantity;
        UnitPrice = unitPrice;
        DiscountPercentage = DiscountRules.Resolve(quantity);
        DiscountAmount = new Money(decimal.Round(unitPrice.Amount * quantity * DiscountPercentage.Value / 100m, 2));
        Total = new Money(decimal.Round(unitPrice.Amount * quantity - DiscountAmount.Amount, 2));
    }

    public void Cancel()
    {
        if (Cancelled) return;
        Cancelled = true;
    }
}

