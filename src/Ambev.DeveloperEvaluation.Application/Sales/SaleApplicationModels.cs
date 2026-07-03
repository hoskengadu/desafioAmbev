namespace Ambev.DeveloperEvaluation.Application.Sales;

public sealed record SaleItemResponse(
    Guid Id,
    Guid ProductId,
    string ProductName,
    int Quantity,
    decimal UnitPrice,
    decimal DiscountPercentage,
    decimal DiscountAmount,
    decimal Total,
    bool Cancelled);

public sealed record SaleResponse(
    Guid Id,
    string SaleNumber,
    DateTime SaleDate,
    Guid CustomerId,
    string CustomerName,
    Guid BranchId,
    string BranchName,
    decimal TotalAmount,
    bool Cancelled,
    DateTime CreatedAt,
    DateTime? UpdatedAt,
    IReadOnlyCollection<SaleItemResponse> Items);

