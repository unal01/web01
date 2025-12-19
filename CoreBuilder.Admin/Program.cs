using AspNetCoreRateLimit;
using CoreBuilder.Data;
using CoreBuilder.Factories;
using CoreBuilder.GraphQL;
using CoreBuilder.Services;
using CoreBuilder.SignalR;
using CoreBuilder.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using System.Text;
using System.Text.Json.Serialization;

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .WriteTo.Console()
    .WriteTo.File("logs/corebuilder-.log", rollingInterval: RollingInterval.Day)
    .CreateLogger();

try
{
    Log.Information("🚀 CoreBuilder başlatılıyor...");
    var builder = WebApplication.CreateBuilder(args);
    builder.Host.UseSerilog();

    builder.Services.AddDbContext<AppDbContext>(options =>
        options.UseInMemoryDatabase("CoreBuilderDb"));

    builder.Services.AddDistributedMemoryCache();
    builder.Services.AddMemoryCache();
    builder.Services.Configure<IpRateLimitOptions>(builder.Configuration.GetSection("IpRateLimit"));
    builder.Services.Configure<IpRateLimitPolicies>(builder.Configuration.GetSection("IpRateLimitPolicies"));
    builder.Services.AddInMemoryRateLimiting();
    builder.Services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();

    var jwtKey = builder.Configuration["Jwt:Key"] ?? "YourSecretKeyHere123456789";
    builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
            };
        });
    builder.Services.AddAuthorization();

    // SERVICES
    builder.Services.AddScoped<IFileStorageService, InMemoryFileStorageService>();
    builder.Services.AddScoped<SiteGenerationService>();
    builder.Services.AddScoped<ThemeService>();
    builder.Services.AddScoped<SiteService>();
    builder.Services.AddScoped<EducationSiteFactory>();
    builder.Services.AddScoped<MarketingSiteFactory>();
    builder.Services.AddScoped<ImageService>();
    builder.Services.AddScoped<UserService>();
    builder.Services.AddScoped<SettingsService>();
    builder.Services.AddScoped<ContentService>();

    builder.Services.AddSignalR();
    builder.Services.AddGraphQLServer()
        .AddQueryType<Query>()
        .AddMutationType<Mutation>()
        .AddProjections()
        .AddFiltering()
        .AddSorting();

    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();

    var coreAssembly = typeof(CoreBuilder.Controllers.SitesController).Assembly;
    builder.Services.AddControllers()
        .AddApplicationPart(coreAssembly)
        .AddJsonOptions(o => o.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);

    builder.Services.AddRazorPages();
    builder.Services.AddServerSideBlazor();
    builder.Services.AddCors();
    builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(sp.GetRequiredService<Microsoft.AspNetCore.Components.NavigationManager>().BaseUri) });
    builder.Services.AddHealthChecks().AddDbContextCheck<AppDbContext>();

    var app = builder.Build();

    if (app.Environment.IsDevelopment())
    {
        app.UseDeveloperExceptionPage();
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseSerilogRequestLogging();
    app.UseStaticFiles();
    app.UseRouting();
    app.UseAuthentication();
    app.UseAuthorization();

    app.MapControllers();
    app.MapBlazorHub();
    app.MapHub<NotificationHub>("/hubs/notifications");
    app.MapGraphQL("/graphql");
    app.MapHealthChecks("/health");
    app.MapFallbackToPage("/_Host");

    // ===== SEED DATA =====
    using (var scope = app.Services.CreateScope())
    {
        var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        db.Database.EnsureCreated();

        // 1. THEMES
        if (!db.Themes.Any())
        {
            db.Themes.AddRange(
                new Theme { Name = "Okyanus Mavisi", PrimaryColor = "#007bff", SecondaryColor = "#6c757d", FontFamily = "Arial", IsDefault = true },
                new Theme { Name = "Orman Yeşili", PrimaryColor = "#28a745", SecondaryColor = "#155724", FontFamily = "Verdana" },
                new Theme { Name = "Gece Modu", PrimaryColor = "#343a40", SecondaryColor = "#6c757d", FontFamily = "Segoe UI" }
            );
            await db.SaveChangesAsync();
            Log.Information("✅ 3 tema eklendi");
        }

        // 2. ADMIN USER
        var userService = scope.ServiceProvider.GetRequiredService<UserService>();
        if (!db.Users.Any())
        {
            await userService.CreateUserAsync("admin", "admin@corebuilder.com", "Admin123!", "Admin", true);
            Log.Information("✅ Admin: admin / Admin123!");
        }

        // 3. DEMO TENANT
        Guid demoTenantId = Guid.Empty;
        var defaultTheme = await db.Themes.FirstOrDefaultAsync(t => t.IsDefault);
        
        if (defaultTheme == null)
        {
            Log.Error("❌ Varsayılan tema bulunamadı!");
        }
        else if (!db.Tenants.Any())
        {
            var tenant = new Tenant
            {
                Name = "Demo Okul",
                Domain = "demo.localhost",
                Category = "Education",
                ThemeId = defaultTheme.Id
            };
            db.Tenants.Add(tenant);
            await db.SaveChangesAsync();
            demoTenantId = tenant.Id;
            Log.Information("✅ Demo Okul tenant (demo.localhost)");
        }
        else
        {
            demoTenantId = db.Tenants.First().Id;
            Log.Information("ℹ️ Mevcut tenant kullanılıyor");
        }

        // 4. DEMO SLIDERS
        if (demoTenantId != Guid.Empty && !db.Sliders.Any())
        {
            db.Sliders.AddRange(
                new Slider
                {
                    TenantId = demoTenantId,
                    Title = "Modern Eğitim Platformu",
                    Description = "Öğrencileriniz için en iyi dijital öğrenme deneyimi",
                    ImageUrl = "https://images.unsplash.com/photo-1523050854058-8df90110c9f1?w=1200&h=400&fit=crop",
                    ButtonText = "Hemen Başla",
                    ButtonLink = "/kayit",
                    Order = 1,
                    IsActive = true
                },
                new Slider
                {
                    TenantId = demoTenantId,
                    Title = "Online Kurslar",
                    Description = "100+ uzman eğitmen ile 500+ çevrimiçi kurs",
                    ImageUrl = "https://images.unsplash.com/photo-1501504905252-473c47e087f8?w=1200&h=400&fit=crop",
                    ButtonText = "Kursları İncele",
                    ButtonLink = "/kurslar",
                    Order = 2,
                    IsActive = true
                },
                new Slider
                {
                    TenantId = demoTenantId,
                    Title = "Sertifika Programları",
                    Description = "Uluslararası geçerliliğe sahip sertifikalar kazanın",
                    ImageUrl = "https://images.unsplash.com/photo-1434030216411-0b793f4b4173?w=1200&h=400&fit=crop",
                    ButtonText = "Detaylı Bilgi",
                    ButtonLink = "/sertifika",
                    Order = 3,
                    IsActive = true
                }
            );
            await db.SaveChangesAsync();
            Log.Information("✅ 3 slider eklendi");
        }

        // 5. DEMO ANNOUNCEMENTS
        if (demoTenantId != Guid.Empty && !db.Announcements.Any())
        {
            db.Announcements.AddRange(
                new Announcement
                {
                    TenantId = demoTenantId,
                    Title = "Yeni Dönem Kayıtları Başladı!",
                    Content = "2024 Bahar dönemi kayıtlarımız 15 Ocak'ta başlıyor. Erken kayıt fırsatlarından yararlanmak için hemen başvurun. İlk 100 öğrenciye %20 indirim!",
                    ImageUrl = "https://images.unsplash.com/photo-1427504494785-3a9ca7044f45?w=600&h=300&fit=crop",
                    IsImportant = true
                },
                new Announcement
                {
                    TenantId = demoTenantId,
                    Title = "Online Sınav Takvimi",
                    Content = "Final sınavları 20-30 Haziran tarihleri arasında online olarak gerçekleştirilecektir.",
                    ImageUrl = "https://images.unsplash.com/photo-1606326608606-aa0b62935f2b?w=600&h=300&fit=crop"
                },
                new Announcement
                {
                    TenantId = demoTenantId,
                    Title = "Ücretsiz Oryantasyon",
                    Content = "Yeni öğrencilerimiz için ücretsiz oryantasyon programı 1 Şubat'ta başlıyor.",
                    ImageUrl = "https://images.unsplash.com/photo-1523580494863-6f3031224c94?w=600&h=300&fit=crop"
                },
                new Announcement
                {
                    TenantId = demoTenantId,
                    Title = "Kütüphane Bakım",
                    Content = "Dijital kütüphane 10-12 Ocak tarihleri arasında bakımda.",
                    ImageUrl = "https://images.unsplash.com/photo-1481627834876-b7833e8f5570?w=600&h=300&fit=crop"
                },
                new Announcement
                {
                    TenantId = demoTenantId,
                    Title = "Burs Başvuruları Uzatıldı!",
                    Content = "2024 burs başvuruları 31 Ocak'a kadar. Başarılı öğrenciler için tam burs fırsatları!",
                    ImageUrl = "https://images.unsplash.com/photo-1579621970563-ebec7560ff3e?w=600&h=300&fit=crop",
                    IsImportant = true
                }
            );
            await db.SaveChangesAsync();
            Log.Information("✅ 5 duyuru eklendi");
        }

        Log.Information("═══════════════════════════════════");
        Log.Information("🎉 CoreBuilder hazır!");
        Log.Information("📍 https://localhost:5001");
        Log.Information("👤 admin / Admin123!");
        Log.Information("═══════════════════════════════════");
    }

    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "❌ Kritik hata!");
}
finally
{
    Log.CloseAndFlush();
}
