using CoreBuilder.Models;
using CoreBuilder.Services;
using CoreBuilder.Data;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace CoreBuilder.Tests.Services;

public class SiteServiceTests
{
    private AppDbContext GetInMemoryDbContext()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        return new AppDbContext(options);
    }

    [Fact]
    public async Task CreateSiteAsync_Should_AddSite_ToDatabase()
    {
        // Arrange
        var context = GetInMemoryDbContext();
        var service = new SiteService(context);
        var tenant = new Tenant
        {
            Name = "Test Site",
            Domain = "test.com",
            Category = "Education",
            ThemeId = Guid.NewGuid()
        };

        // Act
        await service.CreateSiteAsync(tenant);
        var result = await context.Tenants.FirstOrDefaultAsync(t => t.Name == "Test Site");

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Test Site", result.Name);
        Assert.Equal("test.com", result.Domain);
    }

    [Fact]
    public void GetAllSites_Should_ReturnAllSites()
    {
        // Arrange
        var context = GetInMemoryDbContext();
        var service = new SiteService(context);
        
        var themeId = Guid.NewGuid();
        context.Tenants.AddRange(
            new Tenant { Name = "Site1", Domain = "site1.com", Category = "Education", ThemeId = themeId },
            new Tenant { Name = "Site2", Domain = "site2.com", Category = "Marketing", ThemeId = themeId }
        );
        context.SaveChanges();

        // Act
        var sites = service.GetAllSites();

        // Assert
        Assert.Equal(2, sites.Count);
    }

    [Fact]
    public async Task DeleteSiteAsync_Should_RemoveSite()
    {
        // Arrange
        var context = GetInMemoryDbContext();
        var service = new SiteService(context);
        var tenant = new Tenant
        {
            Name = "ToDelete",
            Domain = "delete.com",
            Category = "Education",
            ThemeId = Guid.NewGuid()
        };
        
        context.Tenants.Add(tenant);
        await context.SaveChangesAsync();

        // Act
        await service.DeleteSiteAsync(tenant.Id);
        var result = await context.Tenants.FindAsync(tenant.Id);

        // Assert
        Assert.Null(result);
    }

    [Theory]
    [InlineData("Education")]
    [InlineData("Marketing")]
    public void CreateSite_Should_AcceptValidCategories(string category)
    {
        // Arrange
        var context = GetInMemoryDbContext();
        var service = new SiteService(context);
        var tenant = new Tenant
        {
            Name = $"Test {category}",
            Domain = $"{category.ToLower()}.com",
            Category = category,
            ThemeId = Guid.NewGuid()
        };

        // Act
        service.CreateSite(tenant);
        var result = context.Tenants.FirstOrDefault(t => t.Category == category);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(category, result.Category);
    }
}
