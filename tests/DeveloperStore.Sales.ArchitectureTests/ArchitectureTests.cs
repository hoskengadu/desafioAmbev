using System.Reflection;
using Xunit;

namespace DeveloperStore.Sales.ArchitectureTests;

public sealed class ArchitectureTests
{
    [Fact]
    public void Domain_should_not_reference_application()
    {
        var references = Assembly.Load("DeveloperStore.Sales.Domain").GetReferencedAssemblies().Select(x => x.Name);
        Assert.DoesNotContain("DeveloperStore.Sales.Application", references);
    }

    [Fact]
    public void Application_should_not_reference_api()
    {
        var references = Assembly.Load("DeveloperStore.Sales.Application").GetReferencedAssemblies().Select(x => x.Name);
        Assert.DoesNotContain("DeveloperStore.Sales.Api", references);
    }
}
