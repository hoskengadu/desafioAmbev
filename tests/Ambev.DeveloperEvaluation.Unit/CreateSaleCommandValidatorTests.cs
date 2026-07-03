using Ambev.DeveloperEvaluation.Application.Sales;

namespace Ambev.DeveloperEvaluation.Unit;

public sealed class CreateSaleCommandValidatorTests
{
    [Fact]
    public void Validator_should_accept_valid_command()
    {
        var validator = new CreateSaleCommandValidator();
        var command = new CreateSaleCommand(
            "S-1",
            DateTime.UtcNow,
            Guid.NewGuid(),
            "Customer",
            Guid.NewGuid(),
            "Branch",
            [new SaleItemRequest(Guid.NewGuid(), "Product", 4, 10)]);

        var result = validator.Validate(command);
        Assert.True(result.IsValid);
    }
}

