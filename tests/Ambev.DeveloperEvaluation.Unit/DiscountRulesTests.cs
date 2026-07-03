using Ambev.DeveloperEvaluation.Domain.Common;
using Ambev.DeveloperEvaluation.Domain.Sales;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit;

public sealed class DiscountRulesTests
{
    [Theory]
    [InlineData(1, 0)]
    [InlineData(4, 10)]
    [InlineData(10, 20)]
    public void Resolve_should_return_expected_discount(int quantity, decimal expected)
    {
        var percentage = DiscountRules.Resolve(quantity);
        Assert.Equal(expected, percentage.Value);
    }

    [Fact]
    public void Resolve_should_throw_when_quantity_above_limit()
        => Assert.Throws<DomainException>(() => DiscountRules.Resolve(21));
}

