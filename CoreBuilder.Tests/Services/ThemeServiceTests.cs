using CoreBuilder.Models;
using CoreBuilder.Services;
using CoreBuilder.Data;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace CoreBuilder.Tests.Services;

public class ThemeServiceTests
{
    private AppDbContext GetInMemoryDbContext()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        return new AppDbContext(options);
    }

    [Fact]
    public async Task CreateThemeAsync_Should_AddTheme_ToDatabase()
    {
        // Arrange
        var context = GetInMemoryDbContext();
        var service = new ThemeService(context);
        var theme = new Theme
        {
            Name = "Test Theme",
            PrimaryColor = "#FF0000",
            SecondaryColor = "#00FF00",
            FontFamily = "Arial",
            IsDefault = false
        };

        // Act
        await service.CreateThemeAsync(theme);
        var result = await context.Themes.FirstOrDefaultAsync(t => t.Name == "Test Theme");

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Test Theme", result.Name);
        Assert.Equal("#FF0000", result.PrimaryColor);
    }

    [Fact]
    public void GetAllThemes_Should_ReturnAllThemes()
    {
        // Arrange
        var context = GetInMemoryDbContext();
        var service = new ThemeService(context);
        
        context.Themes.AddRange(
            new Theme { Name = "Theme1", PrimaryColor = "#000", SecondaryColor = "#FFF", FontFamily = "Arial" },
            new Theme { Name = "Theme2", PrimaryColor = "#111", SecondaryColor = "#EEE", FontFamily = "Verdana" }
        );
        context.SaveChanges();

        // Act
        var themes = service.GetAllThemes();

        // Assert
        Assert.Equal(2, themes.Count);
    }

    [Fact]
    public async Task DeleteThemeAsync_Should_RemoveTheme()
    {
        // Arrange
        var context = GetInMemoryDbContext();
        var service = new ThemeService(context);
        var theme = new Theme
        {
            Name = "ToDelete",
            PrimaryColor = "#000",
            SecondaryColor = "#FFF",
            FontFamily = "Arial"
        };
        
        context.Themes.Add(theme);
        await context.SaveChangesAsync();

        // Act
        await service.DeleteThemeAsync(theme.Id);
        var result = await context.Themes.FindAsync(theme.Id);

        // Assert
        Assert.Null(result);
    }
}
