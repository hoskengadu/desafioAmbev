using Ambev.DeveloperEvaluation.Domain.Sales;
using Ambev.DeveloperEvaluation.Domain.ValueObjects;
using Ambev.DeveloperEvaluation.ORM.Persistence;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;

namespace Ambev.DeveloperEvaluation.Integration;

public sealed class SaleRepositoryTests : IClassFixture<DbFixture>
{
    private readonly DbFixture _fixture;

    public SaleRepositoryTests(DbFixture fixture) => _fixture = fixture;

    [Fact]
    public async Task Should_persist_and_reload_sale_through_repository()
    {
        await using var context = _fixture.CreateContext();
        await context.Database.MigrateAsync();

        var repository = new SaleRepository(context);
        var sale = BuildSale();

        await repository.AddAsync(sale, CancellationToken.None);
        await context.SaveChangesAsync();

        var loaded = await repository.GetByIdAsync(sale.Id, CancellationToken.None);

        loaded.Should().NotBeNull();
        loaded!.SaleNumber.Value.Should().Be("S-1001");
        loaded.SaleItems.Should().HaveCount(1);
        loaded.TotalAmount.Amount.Should().Be(40m);
    }

    private static Sale BuildSale()
    {
        return new Sale(
            new SaleNumber("S-1001"),
            DateTime.UtcNow,
            new Customer(Guid.Parse("11111111-1111-1111-1111-111111111111"), "Customer A"),
            new Branch(Guid.Parse("22222222-2222-2222-2222-222222222222"), "Branch A"),
            [
                new SaleItem(
                    Guid.Parse("33333333-3333-3333-3333-333333333333"),
                    "Product A",
                    4,
                    new Money(10m))
            ]);
    }
}

