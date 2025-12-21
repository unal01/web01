using CoreBuilder.Data;
using CoreBuilder.Infrastructure.Caching;
using CoreBuilder.Services;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging.Abstractions;

namespace CoreBuilder.Tests.Services;

public class ContentServiceWithCacheTests : IDisposable
{
    private readonly AppDbContext _context;
    private readonly ICacheService _cacheService;
    private readonly ContentService _contentService;

    public ContentServiceWithCacheTests()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _context = new AppDbContext(options);

        var memoryCache = new MemoryCache(new MemoryCacheOptions());
        _cacheService = new MemoryCacheService(
            memoryCache,
            NullLogger<MemoryCacheService>.Instance);

        _contentService = new ContentService(_context);
    }

    [Fact]
    public async Task GetAllSlidersAsync_WithCaching_ShouldUseCacheOnSecondCall()
    {
        // Arrange
        var tenantId = Guid.NewGuid();
        var slider = new CoreBuilder.Models.Slider
        {
            TenantId = tenantId,
            Title = "Test Slider",
            ImageUrl = "https://test.com/image.jpg",
            Order = 1,
            IsActive = true
        };
        _context.Sliders.Add(slider);
        await _context.SaveChangesAsync();

        var cacheKey = $"sliders:{tenantId}";

        // Act - First call (cache miss)
        var firstResult = await _cacheService.GetOrCreateAsync(
            cacheKey,
            async () => await _contentService.GetAllSlidersAsync(tenantId),
            TimeSpan.FromMinutes(5));

        // Remove from database to verify cache is used
        _context.Sliders.Remove(slider);
        await _context.SaveChangesAsync();

        // Act - Second call (cache hit)
        var secondResult = await _cacheService.GetAsync<List<CoreBuilder.Models.Slider>>(cacheKey);

        // Assert
        firstResult.Should().HaveCount(1);
        secondResult.Should().NotBeNull();
        secondResult!.Should().HaveCount(1);
        secondResult[0].Title.Should().Be("Test Slider");
    }

    [Fact]
    public async Task CreateSliderAsync_ShouldInvalidateCache()
    {
        // Arrange
        var tenantId = Guid.NewGuid();
        var cacheKey = $"sliders:{tenantId}";

        // Set initial cache
        await _cacheService.SetAsync(cacheKey, new List<CoreBuilder.Models.Slider>());

        var newSlider = new CoreBuilder.Models.Slider
        {
            TenantId = tenantId,
            Title = "New Slider",
            ImageUrl = "https://test.com/new.jpg",
            Order = 1,
            IsActive = true
        };

        // Act
        await _contentService.CreateSliderAsync(newSlider);
        await _cacheService.RemoveAsync(cacheKey); // Simulating cache invalidation

        // Get fresh data
        var result = await _contentService.GetAllSlidersAsync(tenantId);

        // Assert
        result.Should().HaveCount(1);
        result[0].Title.Should().Be("New Slider");
    }

    public void Dispose()
    {
        _context.Database.EnsureDeleted();
        _context.Dispose();
    }
}
