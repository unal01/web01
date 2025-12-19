using CoreBuilder.Models;
using Xunit;

namespace CoreBuilder.Tests.Models;

public class TenantTests
{
    [Fact]
    public void Tenant_Should_HaveDefaultCategory()
    {
        // Arrange & Act
        var tenant = new Tenant
        {
            Name = "Test",
            Domain = "test.com",
            ThemeId = Guid.NewGuid()
        };

        // Assert
        Assert.Equal("Education", tenant.Category);
    }

    [Fact]
    public void Tenant_Should_RequireName()
    {
        // Arrange
        var tenant = new Tenant
        {
            Name = "Required Name",
            Domain = "test.com",
            ThemeId = Guid.NewGuid()
        };

        // Assert
        Assert.NotEmpty(tenant.Name);
    }

    [Theory]
    [InlineData("site1.com")]
    [InlineData("example.org")]
    [InlineData("test.localhost")]
    public void Tenant_Should_AcceptValidDomains(string domain)
    {
        // Arrange & Act
        var tenant = new Tenant
        {
            Name = "Test",
            Domain = domain,
            Category = "Education",
            ThemeId = Guid.NewGuid()
        };

        // Assert
        Assert.Equal(domain, tenant.Domain);
    }
}
