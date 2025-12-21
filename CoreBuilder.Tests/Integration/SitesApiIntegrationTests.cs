using CoreBuilder.Models;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;

namespace CoreBuilder.Tests.Integration;

public class SitesApiIntegrationTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;
    private readonly HttpClient _client;

    public SitesApiIntegrationTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory;
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task GetAllSites_ShouldReturnOk()
    {
        // Act
        var response = await _client.GetAsync("/api/sites");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var sites = await response.Content.ReadFromJsonAsync<List<Tenant>>();
        sites.Should().NotBeNull();
    }

    [Fact]
    public async Task CreateSite_WithValidData_ShouldReturnCreated()
    {
        // Arrange
        var newSite = new
        {
            Name = "Test Site",
            Domain = "test.local",
            Category = "Education",
            ThemeId = Guid.NewGuid()
        };

        var content = new StringContent(
            JsonSerializer.Serialize(newSite),
            Encoding.UTF8,
            "application/json");

        // Act
        var response = await _client.PostAsync("/api/sites", content);

        // Assert
        response.StatusCode.Should().BeOneOf(HttpStatusCode.Created, HttpStatusCode.OK);
    }

    [Fact]
    public async Task GetSite_WithInvalidId_ShouldReturnNotFound()
    {
        // Arrange
        var invalidId = Guid.NewGuid();

        // Act
        var response = await _client.GetAsync($"/api/sites/{invalidId}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task HealthCheck_ShouldReturnHealthy()
    {
        // Act
        var response = await _client.GetAsync("/health/live");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var content = await response.Content.ReadAsStringAsync();
        content.Should().Contain("Healthy");
    }

    [Fact]
    public async Task Swagger_ShouldBeAccessible()
    {
        // Act
        var response = await _client.GetAsync("/swagger/index.html");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task RateLimit_ShouldReturnTooManyRequests_WhenExceeded()
    {
        // Arrange
        var requests = new List<Task<HttpResponseMessage>>();

        // Act - Send 150 requests (limit is 100/min)
        for (int i = 0; i < 150; i++)
        {
            requests.Add(_client.GetAsync("/api/sites"));
        }

        var responses = await Task.WhenAll(requests);

        // Assert - At least one should be rate limited
        responses.Should().Contain(r => r.StatusCode == HttpStatusCode.TooManyRequests);
    }
}
