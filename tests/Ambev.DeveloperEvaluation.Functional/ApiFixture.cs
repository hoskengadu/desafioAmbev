using Ambev.DeveloperEvaluation.WebApi;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Testcontainers.MsSql;

namespace Ambev.DeveloperEvaluation.Functional;

public sealed class ApiFixture : IAsyncLifetime
{
    public MsSqlContainer SqlContainer { get; } = new MsSqlBuilder("mcr.microsoft.com/mssql/server:2022-latest").Build();
    public WebApplicationFactory<Program> Factory { get; private set; } = null!;

    public async Task InitializeAsync()
    {
        await SqlContainer.StartAsync();
        Factory = new WebApplicationFactory<Program>()
            .WithWebHostBuilder(builder =>
            {
                builder.ConfigureAppConfiguration((_, config) =>
                {
                    config.AddInMemoryCollection(new Dictionary<string, string?>
                    {
                        ["ConnectionStrings:SalesDb"] = SqlContainer.GetConnectionString()
                    });
                });
            });
    }

    public async Task DisposeAsync()
    {
        await SqlContainer.DisposeAsync();
    }
}

