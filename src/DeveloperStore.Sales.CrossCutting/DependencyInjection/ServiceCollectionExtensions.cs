using DeveloperStore.Sales.Application.Common;
using DeveloperStore.Sales.Application.Sales;
using DeveloperStore.Sales.Infrastructure.DependencyInjection;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace DeveloperStore.Sales.CrossCutting.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddSales(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddInfrastructure(configuration);
        services.AddValidatorsFromSalesApplication();
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(CreateSaleCommand).Assembly));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
        return services;
    }

    private static IServiceCollection AddValidatorsFromSalesApplication(this IServiceCollection services)
    {
        var assembly = typeof(CreateSaleCommand).Assembly;
        var validators = assembly
            .DefinedTypes
            .Where(type => !type.IsAbstract && !type.IsInterface)
            .SelectMany(type => type.ImplementedInterfaces
                .Where(@interface => @interface.IsGenericType && @interface.GetGenericTypeDefinition() == typeof(IValidator<>))
                .Select(@interface => new { Service = @interface, Implementation = type.AsType() }));

        foreach (var validator in validators)
        {
            services.AddTransient(validator.Service, validator.Implementation);
        }

        return services;
    }
}
