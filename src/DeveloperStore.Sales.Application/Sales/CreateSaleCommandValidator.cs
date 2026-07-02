using DeveloperStore.Sales.Domain.Common;
using FluentValidation;

namespace DeveloperStore.Sales.Application.Sales;

public sealed class CreateSaleCommandValidator : AbstractValidator<CreateSaleCommand>
{
    public CreateSaleCommandValidator()
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
    }
}
