using CoreBuilder.Infrastructure.Middleware;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging.Abstractions;
using System.Text.Json;

namespace CoreBuilder.Tests.Infrastructure;

public class GlobalExceptionMiddlewareTests
{
    [Fact]
    public async Task InvokeAsync_ShouldContinue_WhenNoException()
    {
        // Arrange
        var middleware = CreateMiddleware(async context =>
        {
            context.Response.StatusCode = 200;
            await context.Response.WriteAsync("Success");
        });

        var context = new DefaultHttpContext();
        context.Response.Body = new MemoryStream();

        // Act
        await middleware.InvokeAsync(context);

        // Assert
        context.Response.StatusCode.Should().Be(200);
    }

    [Fact]
    public async Task InvokeAsync_ShouldHandle_ValidationException()
    {
        // Arrange
        var errors = new Dictionary<string, string[]>
        {
            { "Email", new[] { "Email is required" } }
        };

        var middleware = CreateMiddleware(_ =>
            throw new ValidationException(errors));

        var context = new DefaultHttpContext();
        context.Response.Body = new MemoryStream();

        // Act
        await middleware.InvokeAsync(context);

        // Assert
        context.Response.StatusCode.Should().Be(400);
        
        context.Response.Body.Seek(0, SeekOrigin.Begin);
        var responseBody = await new StreamReader(context.Response.Body).ReadToEndAsync();
        var response = JsonSerializer.Deserialize<ErrorResponse>(responseBody, 
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        response.Should().NotBeNull();
        response!.StatusCode.Should().Be(400);
        response.Message.Should().Be("Validation failed");
        response.Errors.Should().ContainKey("Email");
    }

    [Fact]
    public async Task InvokeAsync_ShouldHandle_UnauthorizedAccessException()
    {
        // Arrange
        var middleware = CreateMiddleware(_ =>
            throw new UnauthorizedAccessException());

        var context = new DefaultHttpContext();
        context.Response.Body = new MemoryStream();

        // Act
        await middleware.InvokeAsync(context);

        // Assert
        context.Response.StatusCode.Should().Be(401);
    }

    [Fact]
    public async Task InvokeAsync_ShouldHandle_KeyNotFoundException()
    {
        // Arrange
        var middleware = CreateMiddleware(_ =>
            throw new KeyNotFoundException());

        var context = new DefaultHttpContext();
        context.Response.Body = new MemoryStream();

        // Act
        await middleware.InvokeAsync(context);

        // Assert
        context.Response.StatusCode.Should().Be(404);
    }

    [Fact]
    public async Task InvokeAsync_ShouldHandle_GeneralException()
    {
        // Arrange
        var middleware = CreateMiddleware(_ =>
            throw new Exception("Unexpected error"));

        var context = new DefaultHttpContext();
        context.Response.Body = new MemoryStream();

        // Act
        await middleware.InvokeAsync(context);

        // Assert
        context.Response.StatusCode.Should().Be(500);
        
        context.Response.Body.Seek(0, SeekOrigin.Begin);
        var responseBody = await new StreamReader(context.Response.Body).ReadToEndAsync();
        var response = JsonSerializer.Deserialize<ErrorResponse>(responseBody,
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        response.Should().NotBeNull();
        response!.Message.Should().Be("An internal server error occurred");
    }

    [Fact]
    public async Task InvokeAsync_ShouldIncludeDetails_InDevelopment()
    {
        // Arrange
        var exceptionMessage = "Detailed error message";
        var middleware = new GlobalExceptionMiddleware(
            _ => throw new Exception(exceptionMessage),
            NullLogger<GlobalExceptionMiddleware>.Instance,
            new FakeHostEnvironment { EnvironmentName = "Development" });

        var context = new DefaultHttpContext();
        context.Response.Body = new MemoryStream();

        // Act
        await middleware.InvokeAsync(context);

        // Assert
        context.Response.Body.Seek(0, SeekOrigin.Begin);
        var responseBody = await new StreamReader(context.Response.Body).ReadToEndAsync();
        var response = JsonSerializer.Deserialize<ErrorResponse>(responseBody,
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        response.Should().NotBeNull();
        response!.Details.Should().Be(exceptionMessage);
        response.StackTrace.Should().NotBeNullOrEmpty();
    }

    [Fact]
    public async Task InvokeAsync_ShouldNotIncludeDetails_InProduction()
    {
        // Arrange
        var middleware = new GlobalExceptionMiddleware(
            _ => throw new Exception("Sensitive error"),
            NullLogger<GlobalExceptionMiddleware>.Instance,
            new FakeHostEnvironment { EnvironmentName = "Production" });

        var context = new DefaultHttpContext();
        context.Response.Body = new MemoryStream();

        // Act
        await middleware.InvokeAsync(context);

        // Assert
        context.Response.Body.Seek(0, SeekOrigin.Begin);
        var responseBody = await new StreamReader(context.Response.Body).ReadToEndAsync();
        var response = JsonSerializer.Deserialize<ErrorResponse>(responseBody,
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        response.Should().NotBeNull();
        response!.Details.Should().BeNull();
        response.StackTrace.Should().BeNull();
    }

    private GlobalExceptionMiddleware CreateMiddleware(RequestDelegate next)
    {
        return new GlobalExceptionMiddleware(
            next,
            NullLogger<GlobalExceptionMiddleware>.Instance,
            new FakeHostEnvironment());
    }

    private class FakeHostEnvironment : IHostEnvironment
    {
        public string EnvironmentName { get; set; } = "Production";
        public string ApplicationName { get; set; } = "CoreBuilder.Tests";
        public string ContentRootPath { get; set; } = string.Empty;
        public IFileProvider ContentRootFileProvider { get; set; } = null!;
    }
}
