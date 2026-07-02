using System.Net;

namespace DeveloperStore.Sales.IntegrationTests;

public sealed class SalesApiTests : IClassFixture<ApiFixture>
{
    private readonly ApiFixture _fixture;

    public SalesApiTests(ApiFixture fixture) => _fixture = fixture;

    [Fact]
    public async Task Get_sales_should_return_success_status()
    {
        var client = _fixture.Factory.CreateClient();
        var response = await client.GetAsync("/sales");
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }
}
