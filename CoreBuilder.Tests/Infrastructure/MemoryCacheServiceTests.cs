using CoreBuilder.Infrastructure.Caching;
using FluentAssertions;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging.Abstractions;

namespace CoreBuilder.Tests.Infrastructure;

public class MemoryCacheServiceTests
{
    private readonly ICacheService _cacheService;

    public MemoryCacheServiceTests()
    {
        var memoryCache = new MemoryCache(new MemoryCacheOptions());
        _cacheService = new MemoryCacheService(
            memoryCache,
            NullLogger<MemoryCacheService>.Instance,
            TimeSpan.FromMinutes(5));
    }

    [Fact]
    public async Task SetAsync_ShouldStoreValue()
    {
        // Arrange
        var key = "test-key";
        var value = "test-value";

        // Act
        await _cacheService.SetAsync(key, value);
        var result = await _cacheService.GetAsync<string>(key);

        // Assert
        result.Should().Be(value);
    }

    [Fact]
    public async Task GetAsync_ShouldReturnNull_WhenKeyDoesNotExist()
    {
        // Arrange
        var key = "non-existent-key";

        // Act
        var result = await _cacheService.GetAsync<string>(key);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task RemoveAsync_ShouldDeleteValue()
    {
        // Arrange
        var key = "test-key";
        var value = "test-value";
        await _cacheService.SetAsync(key, value);

        // Act
        await _cacheService.RemoveAsync(key);
        var result = await _cacheService.GetAsync<string>(key);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task ExistsAsync_ShouldReturnTrue_WhenKeyExists()
    {
        // Arrange
        var key = "test-key";
        var value = "test-value";
        await _cacheService.SetAsync(key, value);

        // Act
        var exists = await _cacheService.ExistsAsync(key);

        // Assert
        exists.Should().BeTrue();
    }

    [Fact]
    public async Task ExistsAsync_ShouldReturnFalse_WhenKeyDoesNotExist()
    {
        // Arrange
        var key = "non-existent-key";

        // Act
        var exists = await _cacheService.ExistsAsync(key);

        // Assert
        exists.Should().BeFalse();
    }

    [Fact]
    public async Task GetOrCreateAsync_ShouldReturnCachedValue_WhenExists()
    {
        // Arrange
        var key = "test-key";
        var cachedValue = "cached-value";
        await _cacheService.SetAsync(key, cachedValue);
        var factoryCalled = false;

        // Act
        var result = await _cacheService.GetOrCreateAsync(
            key,
            () =>
            {
                factoryCalled = true;
                return Task.FromResult("new-value");
            });

        // Assert
        result.Should().Be(cachedValue);
        factoryCalled.Should().BeFalse();
    }

    [Fact]
    public async Task GetOrCreateAsync_ShouldCallFactory_WhenNotExists()
    {
        // Arrange
        var key = "test-key";
        var newValue = "new-value";

        // Act
        var result = await _cacheService.GetOrCreateAsync(
            key,
            () => Task.FromResult(newValue));

        // Assert
        result.Should().Be(newValue);
        var cached = await _cacheService.GetAsync<string>(key);
        cached.Should().Be(newValue);
    }

    [Fact]
    public async Task RemoveByPatternAsync_ShouldRemoveMatchingKeys()
    {
        // Arrange
        await _cacheService.SetAsync("user:1", "value1");
        await _cacheService.SetAsync("user:2", "value2");
        await _cacheService.SetAsync("product:1", "value3");

        // Act
        await _cacheService.RemoveByPatternAsync("user:*");

        // Assert
        (await _cacheService.ExistsAsync("user:1")).Should().BeFalse();
        (await _cacheService.ExistsAsync("user:2")).Should().BeFalse();
        (await _cacheService.ExistsAsync("product:1")).Should().BeTrue();
    }

    [Fact]
    public async Task SetAsync_WithExpiration_ShouldExpire()
    {
        // Arrange
        var key = "test-key";
        var value = "test-value";
        var expiration = TimeSpan.FromMilliseconds(100);

        // Act
        await _cacheService.SetAsync(key, value, expiration);
        await Task.Delay(150);
        var result = await _cacheService.GetAsync<string>(key);

        // Assert
        result.Should().BeNull();
    }
}
