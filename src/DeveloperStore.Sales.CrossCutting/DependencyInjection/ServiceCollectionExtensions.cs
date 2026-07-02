using DeveloperStore.Sales.Application.Common;
using DeveloperStore.Sales.Application.Sales;
using DeveloperStore.Sales.Infrastructure.DependencyInjection;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DeveloperStore.Sales.CrossCutting.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddSales(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddInfrastructure(configuration);
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(CreateSaleCommand).Assembly));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
        return services;
    }
}
