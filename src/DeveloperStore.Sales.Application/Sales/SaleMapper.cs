using DeveloperStore.Sales.Domain.Sales;

namespace DeveloperStore.Sales.Application.Sales;

public static class SaleMapper
{
    public static SaleResponse ToResponse(Sale sale) => new(
        sale.Id,
        sale.SaleNumber.Value,
        sale.SaleDate,
        sale.Customer.Id,
        sale.Customer.Name,
        sale.Branch.Id,
        sale.Branch.Name,
        sale.TotalAmount.Amount,
        sale.Cancelled,
        sale.CreatedAt,
        sale.UpdatedAt,
        sale.SaleItems.Select(x => new SaleItemResponse(
            x.Id,
            x.ProductId,
            x.ProductName,
            x.Quantity,
            x.UnitPrice.Amount,
            x.DiscountPercentage.Value,
            x.DiscountAmount.Amount,
            x.Total.Amount,
            x.Cancelled)).ToArray());
}
