using Ambev.DeveloperEvaluation.Application.Sales;

namespace Ambev.DeveloperEvaluation.Unit;

public sealed class GetAllSalesQueryValidatorTests
{
    [Fact]
    public void Validator_should_accept_valid_query()
    {
        var validator = new GetAllSalesQueryValidator();
        var query = new GetAllSalesQuery(PageNumber: 1, PageSize: 10);

        var result = validator.Validate(query);

        Assert.True(result.IsValid);
    }

    [Fact]
    public void Validator_should_reject_invalid_page_size()
    {
        var validator = new GetAllSalesQueryValidator();
        var query = new GetAllSalesQuery(PageNumber: 1, PageSize: 0);

        var result = validator.Validate(query);

        Assert.False(result.IsValid);
    }
}

