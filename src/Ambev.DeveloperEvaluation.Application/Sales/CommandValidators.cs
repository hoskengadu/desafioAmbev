using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Sales;

public sealed class UpdateSaleCommandValidator : AbstractValidator<UpdateSaleCommand>
{
    public UpdateSaleCommandValidator()
    {
        RuleFor(x => x.SaleNumber).NotEmpty();
        RuleFor(x => x.CustomerName).NotEmpty();
        RuleFor(x => x.BranchName).NotEmpty();
        RuleFor(x => x.Items).NotEmpty();
        RuleForEach(x => x.Items).ChildRules(item =>
        {
            item.RuleFor(x => x.ProductId).NotEqual(Guid.Empty);
            item.RuleFor(x => x.ProductName).NotEmpty();
            item.RuleFor(x => x.Quantity).InclusiveBetween(1, 20);
            item.RuleFor(x => x.UnitPrice).GreaterThanOrEqualTo(0);
        });
        RuleFor(x => x.Id).NotEmpty();
    }
}

public sealed class DeleteSaleCommandValidator : AbstractValidator<DeleteSaleCommand>
{
    public DeleteSaleCommandValidator() => RuleFor(x => x.Id).NotEmpty();
}

public sealed class CancelSaleCommandValidator : AbstractValidator<CancelSaleCommand>
{
    public CancelSaleCommandValidator() => RuleFor(x => x.Id).NotEmpty();
}

public sealed class CancelSaleItemCommandValidator : AbstractValidator<CancelSaleItemCommand>
{
    public CancelSaleItemCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.ItemId).NotEmpty();
    }
}

public sealed class GetSaleByIdQueryValidator : AbstractValidator<GetSaleByIdQuery>
{
    public GetSaleByIdQueryValidator() => RuleFor(x => x.Id).NotEmpty();
}

public sealed class GetSaleByNumberQueryValidator : AbstractValidator<GetSaleByNumberQuery>
{
    public GetSaleByNumberQueryValidator() => RuleFor(x => x.SaleNumber).NotEmpty();
}

public sealed class GetAllSalesQueryValidator : AbstractValidator<GetAllSalesQuery>
{
    public GetAllSalesQueryValidator()
    {
        RuleFor(x => x.PageNumber).GreaterThan(0);
        RuleFor(x => x.PageSize).InclusiveBetween(1, 100);

        RuleFor(x => x).Custom((query, context) =>
        {
            if (query.SaleDateFrom.HasValue && query.SaleDateTo.HasValue && query.SaleDateTo.Value < query.SaleDateFrom.Value)
            {
                context.AddFailure(nameof(GetAllSalesQuery.SaleDateTo), "SaleDateTo must be greater than or equal to SaleDateFrom.");
            }
        });
    }
}

