using Microsoft.AspNetCore.Builder;
using DeveloperStore.Sales.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace DeveloperStore.Sales.CrossCutting.DependencyInjection;

public static class ApplicationBuilderExtensions
{
    public static async Task UseSalesDatabaseAsync(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<SalesDbContext>();

        for (var attempt = 1; attempt <= 10; attempt++)
        {
            try
            {
                await context.Database.MigrateAsync();
                return;
            }
            catch when (attempt < 10)
            {
                await Task.Delay(TimeSpan.FromSeconds(3));
            }
        }

        await context.Database.MigrateAsync();
    }
}
