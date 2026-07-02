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
