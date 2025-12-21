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
    Log.Information("CoreBuilder baslatiliyor...");
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

    builder.Services.AddScoped<IFileStorageService, InMemoryFileStorageService>();
    builder.Services.AddScoped<SiteGenerationService>();
    builder.Services.AddScoped<SiteTemplateService>();
    builder.Services.AddScoped<ThemeService>();
    builder.Services.AddScoped<SiteService>();
    builder.Services.AddScoped<EducationSiteFactory>();
    builder.Services.AddScoped<MarketingSiteFactory>();
    builder.Services.AddScoped<GovernmentSiteFactory>();
    builder.Services.AddScoped<ExamPrepSiteFactory>();
    builder.Services.AddScoped<ImageService>();
    builder.Services.AddScoped<UserService>();
    builder.Services.AddScoped<SettingsService>();
    builder.Services.AddScoped<ContentService>();
    builder.Services.AddScoped<TeacherService>();
    builder.Services.AddScoped<PageContentService>();
    builder.Services.AddScoped<FontSettingsService>();

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

        // THEMES
        if (!db.Themes.Any())
        {
            db.Themes.AddRange(
                new Theme { Name = "Okyanus Mavisi", PrimaryColor = "#007bff", SecondaryColor = "#6c757d", FontFamily = "Arial", IsDefault = true },
                new Theme { Name = "Orman Yesili", PrimaryColor = "#28a745", SecondaryColor = "#155724", FontFamily = "Verdana" },
                new Theme { Name = "Turuncu Egitim", PrimaryColor = "#ff6600", SecondaryColor = "#cc5200", FontFamily = "Poppins" }
            );
            await db.SaveChangesAsync();
        }

        // ADMIN USER
        var userService = scope.ServiceProvider.GetRequiredService<UserService>();
        if (!db.Users.Any())
        {
            await userService.CreateUserAsync("admin", "admin@corebuilder.com", "Admin123!", "Admin", true);
        }

        var defaultTheme = await db.Themes.FirstOrDefaultAsync(t => t.IsDefault);
        var orangeTheme = await db.Themes.FirstOrDefaultAsync(t => t.Name == "Turuncu Egitim");

        // BARTIN SINAV ILERI KURS
        if (defaultTheme != null && !db.Tenants.Any(t => t.Name == "Bartin Sinav Ileri Kurs"))
        {
            var tenant = new Tenant
            {
                Name = "Bartin Sinav Ileri Kurs",
                Domain = "sinav.localhost",
                Category = "Education",
                ThemeId = orangeTheme?.Id ?? defaultTheme.Id
            };
            db.Tenants.Add(tenant);
            await db.SaveChangesAsync();

            // SAYFALAR
            var anasayfa = new Page { TenantId = tenant.Id, Title = "Ana Sayfa", Slug = "anasayfa", Content = "", IsPublished = true, MenuOrder = 0 };
            var hakkimizda = new Page { TenantId = tenant.Id, Title = "Hakkimizda", Slug = "hakkimizda", Content = "<h2>Bartin Sinav Ileri Kurs</h2><p>2010 yilindan bu yana Bartin'da egitim veren kurumumuz, binlerce ogrenciyi hayallerindeki universite ve liseye yerlestirmistir.</p>", IsPublished = true, MenuOrder = 1 };
            var kurslar = new Page { TenantId = tenant.Id, Title = "Kurslarimiz", Slug = "kurslarimiz", Content = "<h2>Egitim Programlarimiz</h2><ul><li>YKS Hazirlik</li><li>LGS Hazirlik</li><li>KPSS Hazirlik</li><li>DGS Hazirlik</li></ul>", IsPublished = true, MenuOrder = 2 };
            var iletisim = new Page { TenantId = tenant.Id, Title = "Iletisim", Slug = "iletisim", Content = "", IsPublished = true, MenuOrder = 3 };
            var duyurular = new Page { TenantId = tenant.Id, Title = "Duyurular", Slug = "duyurular", Content = "", IsPublished = true, MenuOrder = 4 };

            db.Pages.AddRange(anasayfa, hakkimizda, kurslar, iletisim, duyurular);
            await db.SaveChangesAsync();

            // PAGE CONTENTS
            db.PageContents.AddRange(
                new PageContent { PageId = anasayfa.Id, ContentType = ContentType.WidgetSlider, Title = "Slider", DisplayOrder = 0, IsActive = true },
                new PageContent { PageId = anasayfa.Id, ContentType = ContentType.WidgetAnnouncements, Title = "Duyurular", DisplayOrder = 1, IsActive = true },
                new PageContent { PageId = anasayfa.Id, ContentType = ContentType.WidgetNews, Title = "Haberler", DisplayOrder = 2, IsActive = true },
                new PageContent { PageId = duyurular.Id, ContentType = ContentType.WidgetAnnouncements, Title = "Duyurular", DisplayOrder = 0, IsActive = true }
            );
            await db.SaveChangesAsync();

            // SLIDERLAR
            db.Sliders.AddRange(
                new Slider
                {
                    TenantId = tenant.Id,
                    Title = "2024 YKS Hazirlik Kayitlari Basladi",
                    Description = "Turkiye'nin en basarili egitim kadrosuyla YKS'ye hazirlanin! Erken kayit indirimi icin acele edin.",
                    ImageUrl = "https://images.unsplash.com/photo-1523050854058-8df90110c9f1?w=1200&h=400&fit=crop",
                    ButtonText = "Hemen Kayit Ol",
                    ButtonLink = "/kayit",
                    Order = 1,
                    IsActive = true
                },
                new Slider
                {
                    TenantId = tenant.Id,
                    Title = "LGS'de Yuzde 100 Basari",
                    Description = "Gecen yil ogrencilerimizin yuzde 85'i ilk 10.000'e girdi!",
                    ImageUrl = "https://images.unsplash.com/photo-1427504494785-3a9ca7044f45?w=1200&h=400&fit=crop",
                    ButtonText = "Basari Hikayelerini Gor",
                    ButtonLink = "/basarilar",
                    Order = 2,
                    IsActive = true
                },
                new Slider
                {
                    TenantId = tenant.Id,
                    Title = "Ucretsiz Deneme Sinavi",
                    Description = "Her hafta sonu ucretsiz deneme sinavlari ile seviyenizi olcun!",
                    ImageUrl = "https://images.unsplash.com/photo-1434030216411-0b793f4b4173?w=1200&h=400&fit=crop",
                    ButtonText = "Sinav Takvimi",
                    ButtonLink = "/sinav-takvimi",
                    Order = 3,
                    IsActive = true
                }
            );
            await db.SaveChangesAsync();

            // DUYURULAR
            db.Announcements.AddRange(
                new Announcement
                {
                    TenantId = tenant.Id,
                    Title = "2024-2025 Egitim Donemi Kayitlari Basladi!",
                    Content = "Yeni egitim donemi kayitlarimiz baslamistir. Erken kayit yaptiran ogrencilerimize yuzde 20 indirim firsati! Son basvuru: 15 Eylul 2024",
                    ImageUrl = "https://images.unsplash.com/photo-1546410531-bb4caa6b424d?w=600&h=300&fit=crop",
                    IsImportant = true,
                    PublishDate = DateTime.UtcNow
                },
                new Announcement
                {
                    TenantId = tenant.Id,
                    Title = "Yaz Kurslari Basliyor",
                    Content = "Temmuz-Agustos aylarinda yogun kamp programimiz basliyor. Kontenjan sinirlidir!",
                    ImageUrl = "https://images.unsplash.com/photo-1503676260728-1c00da094a0b?w=600&h=300&fit=crop",
                    IsImportant = false,
                    PublishDate = DateTime.UtcNow.AddDays(-2)
                },
                new Announcement
                {
                    TenantId = tenant.Id,
                    Title = "Deneme Sinavi Sonuclari Aciklandi",
                    Content = "Gecen hafta yapilan TYT deneme sinavi sonuclari aciklanmistir.",
                    ImageUrl = "https://images.unsplash.com/photo-1606326608606-aa0b62935f2b?w=600&h=300&fit=crop",
                    IsImportant = false,
                    PublishDate = DateTime.UtcNow.AddDays(-5)
                },
                new Announcement
                {
                    TenantId = tenant.Id,
                    Title = "Veli Toplantisi Duyurusu",
                    Content = "25 Aralik Cumartesi gunu saat 14:00'te veli toplantimiz yapilacaktir.",
                    ImageUrl = "https://images.unsplash.com/photo-1577896851231-70ef18881754?w=600&h=300&fit=crop",
                    IsImportant = true,
                    PublishDate = DateTime.UtcNow.AddDays(-1)
                }
            );
            await db.SaveChangesAsync();

            // ILETISIM BILGILERI
            db.ContactInfos.Add(new ContactInfo
            {
                TenantId = tenant.Id,
                Phone1 = "+90 (378) 227 45 67",
                Phone1Label = "Sekreterlik",
                Phone2 = "+90 (505) 656 20 60",
                Phone2Label = "Whatsapp",
                Phone3 = "+90 (378) 227 45 68",
                Phone3Label = "Fax",
                Email = "info@bartinsinavkurs.com",
                Email2 = "kayit@bartinsinavkurs.com",
                Address = "Kemerkopru Mahallesi, Cumhuriyet Caddesi No:45/A",
                District = "Merkez",
                City = "BARTIN",
                PostalCode = "74100",
                WorkingDays = "Pazartesi - Cumartesi",
                WorkingHours = "08:30 - 21:00",
                WhatsApp = "905056562060",
                Instagram = "bartinsinavkurs",
                Facebook = "bartinsinavkurs",
                Twitter = "bartinsinav",
                YouTube = "bartinsinavkurs",
                GoogleMapsEmbedUrl = "https://www.google.com/maps/embed?pb=!1m18!1m12!1m3!1d2984.789!2d32.3375!3d41.6358",
                PageLayout = "layout1"
            });
            await db.SaveChangesAsync();

            Log.Information("Bartin Sinav Ileri Kurs sitesi olusturuldu!");
        }

        Log.Information("==========================================");
        Log.Information("CoreBuilder hazir!");
        Log.Information("https://localhost:5001");
        Log.Information("admin / Admin123!");
        Log.Information("==========================================");
    }

    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Kritik hata!");
}
finally
{
    Log.CloseAndFlush();
}
