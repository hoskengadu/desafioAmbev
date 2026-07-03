using Ambev.DeveloperEvaluation.ORM.Persistence;
using Microsoft.EntityFrameworkCore;
using Testcontainers.MsSql;

namespace Ambev.DeveloperEvaluation.Integration;

public sealed class DbFixture : IAsyncLifetime
{
    public MsSqlContainer SqlContainer { get; } = new MsSqlBuilder("mcr.microsoft.com/mssql/server:2022-latest").Build();

    public async Task InitializeAsync()
    {
        await SqlContainer.StartAsync();
    }

    public async Task DisposeAsync()
    {
        await SqlContainer.DisposeAsync();
    }

    public SalesDbContext CreateContext()
    {
        var options = new DbContextOptionsBuilder<SalesDbContext>()
            .UseSqlServer(SqlContainer.GetConnectionString())
            .Options;

        return new SalesDbContext(options);
    }
}

