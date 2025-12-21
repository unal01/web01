namespace CoreBuilder.Infrastructure.Security;

/// <summary>
/// CORS policy configuration
/// </summary>
public class CorsSettings
{
    public string[] AllowedOrigins { get; set; } = Array.Empty<string>();
    public string[] AllowedMethods { get; set; } = Array.Empty<string>();
    public string[] AllowedHeaders { get; set; } = Array.Empty<string>();
    public bool AllowCredentials { get; set; }
    public int PreflightMaxAge { get; set; } = 600; // 10 minutes
}

public static class CorsExtensions
{
    public const string DefaultPolicyName = "CoreBuilderPolicy";

    public static IServiceCollection AddCorsPolicies(
        this IServiceCollection services, 
        IConfiguration configuration)
    {
        var settings = configuration.GetSection("CorsSettings").Get<CorsSettings>() 
            ?? new CorsSettings();

        services.AddCors(options =>
        {
            options.AddPolicy(DefaultPolicyName, builder =>
            {
                // Development mode - allow all
                if (settings.AllowedOrigins.Contains("*"))
                {
                    builder.AllowAnyOrigin()
                           .AllowAnyMethod()
                           .AllowAnyHeader();
                }
                else
                {
                    builder.WithOrigins(settings.AllowedOrigins)
                           .WithMethods(settings.AllowedMethods.Length > 0 
                               ? settings.AllowedMethods 
                               : new[] { "GET", "POST", "PUT", "DELETE", "OPTIONS" })
                           .WithHeaders(settings.AllowedHeaders.Length > 0 
                               ? settings.AllowedHeaders 
                               : new[] { "Content-Type", "Authorization", "X-Correlation-ID" });

                    if (settings.AllowCredentials)
                    {
                        builder.AllowCredentials();
                    }

                    builder.SetPreflightMaxAge(TimeSpan.FromSeconds(settings.PreflightMaxAge));
                }
            });
        });

        return services;
    }
}
