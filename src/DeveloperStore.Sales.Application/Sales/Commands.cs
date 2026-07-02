namespace DeveloperStore.Sales.Application.Sales;

public sealed record SaleItemDto(Guid ProductId, string ProductName, int Quantity, decimal UnitPrice);

public sealed record CreateSaleCommand(string SaleNumber, DateTime SaleDate, Guid CustomerId, string CustomerName, Guid BranchId, string BranchName, IReadOnlyCollection<SaleItemDto> Items);
public sealed record UpdateSaleCommand(Guid Id, string SaleNumber, DateTime SaleDate, Guid CustomerId, string CustomerName, Guid BranchId, string BranchName, IReadOnlyCollection<SaleItemDto> Items);
public sealed record DeleteSaleCommand(Guid Id);
public sealed record CancelSaleCommand(Guid Id);
public sealed record CancelSaleItemCommand(Guid Id, Guid ItemId);

public sealed record GetSaleByIdQuery(Guid Id);
public sealed record GetAllSalesQuery;
public sealed record GetSaleByNumberQuery(string SaleNumber);
