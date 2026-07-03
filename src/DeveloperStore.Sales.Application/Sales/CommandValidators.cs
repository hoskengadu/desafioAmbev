using FluentValidation;

namespace DeveloperStore.Sales.Application.Sales;

public sealed class UpdateSaleCommandValidator : AbstractValidator<UpdateSaleCommand>
{
    public UpdateSaleCommandValidator()
    {
        Include(new CreateSaleCommandValidator());
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
