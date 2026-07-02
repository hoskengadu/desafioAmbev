using NetArchTest.Rules;

namespace DeveloperStore.Sales.ArchitectureTests;

public sealed class ArchitectureTests
{
    [Fact]
    public void Domain_should_not_reference_application()
    {
        var result = Types.InAssembly(typeof(DeveloperStore.Sales.Domain.Sales.Sale).Assembly)
            .ShouldNot()
            .HaveDependencyOn("DeveloperStore.Sales.Application")
            .GetResult();

        Assert.True(result.IsSuccessful);
    }

    [Fact]
    public void Application_should_not_reference_api()
    {
        var result = Types.InAssembly(typeof(DeveloperStore.Sales.Application.Sales.CreateSaleCommand).Assembly)
            .ShouldNot()
            .HaveDependencyOn("DeveloperStore.Sales.Api")
            .GetResult();

        Assert.True(result.IsSuccessful);
    }

    [Fact]
    public void Api_should_not_reference_infrastructure_directly()
    {
        var result = Types.InAssembly(typeof(DeveloperStore.Sales.Api.Program).Assembly)
            .ShouldNot()
            .HaveDependencyOn("DeveloperStore.Sales.Infrastructure.Persistence")
            .GetResult();

        Assert.True(result.IsSuccessful);
    }
}
