using Serilog.Context;

namespace CoreBuilder.Infrastructure.Middleware;

/// <summary>
/// Request logging middleware with correlation ID
/// </summary>
public class RequestLoggingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<RequestLoggingMiddleware> _logger;

    public RequestLoggingMiddleware(RequestDelegate next, ILogger<RequestLoggingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var correlationId = context.Request.Headers["X-Correlation-ID"].FirstOrDefault()
            ?? Guid.NewGuid().ToString();

        using (LogContext.PushProperty("CorrelationId", correlationId))
        {
            // Use indexer instead of Add to avoid duplicate key warning
            context.Response.Headers["X-Correlation-ID"] = correlationId;

            var startTime = DateTime.UtcNow;

            _logger.LogInformation(
                "HTTP {Method} {Path} started",
                context.Request.Method,
                context.Request.Path);

            try
            {
                await _next(context);

                var elapsed = DateTime.UtcNow - startTime;

                _logger.LogInformation(
                    "HTTP {Method} {Path} completed with {StatusCode} in {ElapsedMs}ms",
                    context.Request.Method,
                    context.Request.Path,
                    context.Response.StatusCode,
                    elapsed.TotalMilliseconds);
            }
            catch (Exception ex)
            {
                var elapsed = DateTime.UtcNow - startTime;

                _logger.LogError(ex,
                    "HTTP {Method} {Path} failed with {StatusCode} in {ElapsedMs}ms",
                    context.Request.Method,
                    context.Request.Path,
                    context.Response.StatusCode,
                    elapsed.TotalMilliseconds);

                throw;
            }
        }
    }
}
