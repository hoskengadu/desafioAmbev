namespace DeveloperStore.Sales.Application.Sales;

public enum SaleSortField
{
    SaleDate,
    SaleNumber,
    CustomerName,
    BranchName,
    TotalAmount,
    CreatedAt
}

public enum SortOrder
{
    Asc,
    Desc
}

public sealed record SaleSearchRequest(
    int PageNumber = 1,
    int PageSize = 10,
    string? SaleNumber = null,
    string? CustomerName = null,
    string? BranchName = null,
    bool? Cancelled = null,
    DateTime? SaleDateFrom = null,
    DateTime? SaleDateTo = null,
    SaleSortField SortBy = SaleSortField.SaleDate,
    SortOrder SortDirection = SortOrder.Desc);
