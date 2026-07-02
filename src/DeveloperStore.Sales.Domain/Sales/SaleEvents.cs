namespace DeveloperStore.Sales.Domain.Sales;

public sealed record SaleCreatedEvent(Guid SaleId);
public sealed record SaleModifiedEvent(Guid SaleId);
public sealed record SaleCancelledEvent(Guid SaleId);
public sealed record ItemCancelledEvent(Guid SaleId, Guid ItemId);
