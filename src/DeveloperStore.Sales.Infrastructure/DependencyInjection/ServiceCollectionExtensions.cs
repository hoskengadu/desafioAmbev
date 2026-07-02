using DeveloperStore.Sales.Application.Abstractions;
using DeveloperStore.Sales.Application.Sales;
using DeveloperStore.Sales.Infrastructure.Events;
using DeveloperStore.Sales.Infrastructure.Persistence;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DeveloperStore.Sales.Infrastructure.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<InMemorySaleStore>();
        services.AddScoped<ISaleRepository, InMemorySaleRepository>();
        services.AddScoped<IUnitOfWork, InMemoryUnitOfWork>();
        services.AddScoped<IEventPublisher, LoggerEventPublisher>();
        return services;
    }

    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<SaleService>();
        return services;
    }
}
