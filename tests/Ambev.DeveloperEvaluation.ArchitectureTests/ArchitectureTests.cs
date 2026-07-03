using NetArchTest.Rules;

namespace Ambev.DeveloperEvaluation.ArchitectureTests;

public sealed class ArchitectureTests
{
    [Fact]
    public void Domain_should_not_reference_application()
    {
        var result = Types.InAssembly(typeof(Ambev.DeveloperEvaluation.Domain.Sales.Sale).Assembly)
            .ShouldNot()
            .HaveDependencyOn("Ambev.DeveloperEvaluation.Application")
            .GetResult();

        Assert.True(result.IsSuccessful);
    }

    [Fact]
    public void Application_should_not_reference_web_api()
    {
        var result = Types.InAssembly(typeof(Ambev.DeveloperEvaluation.Application.Sales.CreateSaleCommand).Assembly)
            .ShouldNot()
            .HaveDependencyOn("Ambev.DeveloperEvaluation.WebApi")
            .GetResult();

        Assert.True(result.IsSuccessful);
    }

    [Fact]
    public void WebApi_should_not_reference_orm_directly()
    {
        var result = Types.InAssembly(typeof(Ambev.DeveloperEvaluation.WebApi.Program).Assembly)
            .ShouldNot()
            .HaveDependencyOn("Ambev.DeveloperEvaluation.ORM.Persistence")
            .GetResult();

        Assert.True(result.IsSuccessful);
    }
}

