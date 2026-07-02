using DeveloperStore.Sales.Api;
using Microsoft.AspNetCore.Mvc.Testing;
using Testcontainers.MsSql;

namespace DeveloperStore.Sales.IntegrationTests;

public sealed class ApiFixture : IAsyncLifetime
{
    public MsSqlContainer SqlContainer { get; } = new MsSqlBuilder().Build();
    public WebApplicationFactory<Program> Factory { get; private set; } = null!;

    public async Task InitializeAsync()
    {
        await SqlContainer.StartAsync();
        Factory = new WebApplicationFactory<Program>();
    }

    public async Task DisposeAsync()
    {
        await SqlContainer.DisposeAsync();
    }
}
