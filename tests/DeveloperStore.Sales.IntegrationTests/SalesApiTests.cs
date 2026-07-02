using System.Net;
using System.Net.Http.Json;
using Bogus;
using DeveloperStore.Sales.Application.Sales;
using FluentAssertions;

namespace DeveloperStore.Sales.IntegrationTests;

public sealed class SalesApiTests : IClassFixture<ApiFixture>
{
    private sealed record SaleItemPayload(Guid ProductId, string ProductName, int Quantity, decimal UnitPrice);
    private sealed record SalePayload(string SaleNumber, DateTime SaleDate, Guid CustomerId, string CustomerName, Guid BranchId, string BranchName, SaleItemPayload[] Items);

    private readonly ApiFixture _fixture;

    public SalesApiTests(ApiFixture fixture) => _fixture = fixture;

    [Fact]
    public async Task Should_create_get_cancel_item_and_cancel_sale()
    {
        var client = _fixture.Factory.CreateClient();
        var payload = BuildPayload(quantity: 4, unitPrice: 10m);

        var createResponse = await client.PostAsJsonAsync("/sales", payload);
        Assert.Equal(HttpStatusCode.Created, createResponse.StatusCode);

        var createdId = await createResponse.Content.ReadFromJsonAsync<Guid>();
        Assert.NotEqual(Guid.Empty, createdId);

        var getResponse = await client.GetAsync($"/sales/{createdId}");
        Assert.Equal(HttpStatusCode.OK, getResponse.StatusCode);

        var sale = await getResponse.Content.ReadFromJsonAsync<SaleResponse>();
        sale.Should().NotBeNull();
        sale!.Items.Should().HaveCount(1);

        var itemId = sale.Items.First().Id;
        var cancelItemResponse = await client.PatchAsync($"/sales/{createdId}/items/{itemId}/cancel", null);
        Assert.Equal(HttpStatusCode.NoContent, cancelItemResponse.StatusCode);

        var cancelSaleResponse = await client.PatchAsync($"/sales/{createdId}/cancel", null);
        Assert.Equal(HttpStatusCode.NoContent, cancelSaleResponse.StatusCode);
    }

    [Fact]
    public async Task Put_sales_should_update_sale()
    {
        var client = _fixture.Factory.CreateClient();
        var createdId = await CreateSaleAsync(client);

        var updatePayload = BuildPayload(quantity: 10, unitPrice: 20m) with
        {
            SaleDate = DateTime.UtcNow.AddDays(1),
            CustomerName = "Updated Customer",
            BranchName = "Updated Branch"
        };

        var updateResponse = await client.PutAsJsonAsync($"/sales/{createdId}", updatePayload);
        Assert.Equal(HttpStatusCode.NoContent, updateResponse.StatusCode);

        var getResponse = await client.GetAsync($"/sales/{createdId}");
        var sale = await getResponse.Content.ReadFromJsonAsync<SaleResponse>();
        Assert.NotNull(sale);
        Assert.Equal("Updated Customer", sale!.CustomerName);
    }

    [Fact]
    public async Task Delete_sales_should_remove_sale()
    {
        var client = _fixture.Factory.CreateClient();
        var createdId = await CreateSaleAsync(client);

        var deleteResponse = await client.DeleteAsync($"/sales/{createdId}");
        Assert.Equal(HttpStatusCode.NoContent, deleteResponse.StatusCode);

        var getResponse = await client.GetAsync($"/sales/{createdId}");
        Assert.Equal(HttpStatusCode.NotFound, getResponse.StatusCode);
    }

    [Fact]
    public async Task Get_sales_should_return_success_status()
    {
        var client = _fixture.Factory.CreateClient();
        var response = await client.GetAsync("/sales");
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task Get_sales_by_number_should_return_sale()
    {
        var client = _fixture.Factory.CreateClient();
        var payload = BuildPayload(quantity: 4, unitPrice: 10m);
        var saleNumber = payload.SaleNumber;

        var response = await client.PostAsJsonAsync("/sales", payload);
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);

        var getResponse = await client.GetAsync($"/sales/number/{saleNumber}");
        Assert.Equal(HttpStatusCode.OK, getResponse.StatusCode);

        var sale = await getResponse.Content.ReadFromJsonAsync<SaleResponse>();
        sale.Should().NotBeNull();
        sale!.SaleNumber.Should().Be(saleNumber);
    }

    private static async Task<Guid> CreateSaleAsync(HttpClient client)
    {
        var payload = BuildPayload(quantity: 4, unitPrice: 10m);

        var response = await client.PostAsJsonAsync("/sales", payload);
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        return (await response.Content.ReadFromJsonAsync<Guid>())!;
    }

    private static SalePayload BuildPayload(int quantity, decimal unitPrice)
    {
        var faker = new Faker();
        return new SalePayload(
            $"S-{faker.Random.AlphaNumeric(10).ToUpperInvariant()}",
            DateTime.UtcNow,
            faker.Random.Guid(),
            faker.Name.FullName(),
            faker.Random.Guid(),
            faker.Company.CompanyName(),
            [
                new SaleItemPayload(
                    faker.Random.Guid(),
                    faker.Commerce.ProductName(),
                    quantity,
                    unitPrice)
            ]);
    }
}
