using DeveloperStore.Sales.Application.Common;
using MediatR;

namespace DeveloperStore.Sales.Application.Sales;

public sealed record SaleItemRequest(Guid ProductId, string ProductName, int Quantity, decimal UnitPrice);

public sealed record CreateSaleCommand(string SaleNumber, DateTime SaleDate, Guid CustomerId, string CustomerName, Guid BranchId, string BranchName, IReadOnlyCollection<SaleItemRequest> Items)
    : IRequest<Result<Guid>>;

public sealed record UpdateSaleCommand(Guid Id, string SaleNumber, DateTime SaleDate, Guid CustomerId, string CustomerName, Guid BranchId, string BranchName, IReadOnlyCollection<SaleItemRequest> Items)
    : IRequest<Result>;

public sealed record DeleteSaleCommand(Guid Id) : IRequest<Result>;
public sealed record CancelSaleCommand(Guid Id) : IRequest<Result>;
public sealed record CancelSaleItemCommand(Guid Id, Guid ItemId) : IRequest<Result>;
public sealed record GetSaleByIdQuery(Guid Id) : IRequest<Result<SaleResponse>>;
public sealed record GetAllSalesQuery() : IRequest<Result<IReadOnlyCollection<SaleResponse>>>;
public sealed record GetSaleByNumberQuery(string SaleNumber) : IRequest<Result<SaleResponse>>;
