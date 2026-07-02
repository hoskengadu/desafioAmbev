using DeveloperStore.Sales.Application.Abstractions;
using DeveloperStore.Sales.Application.Sales;
using DeveloperStore.Sales.Infrastructure.Events;
using DeveloperStore.Sales.Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DeveloperStore.Sales.Infrastructure.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("SalesDb") ?? "Server=localhost,1433;Database=DeveloperStoreSales;User Id=sa;Password=Your_password123;TrustServerCertificate=True";
        services.AddDbContext<SalesDbContext>(options => options.UseSqlServer(connectionString));
        services.AddScoped<ISaleRepository, SaleRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IEventPublisher, LoggerEventPublisher>();
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(CreateSaleCommand).Assembly));
        return services;
    }
}
