# CoreBuilder.Admin/Program.cs Güncelleme K?lavuzu

## Ad?m 1: Using'leri Ekle

Dosyan?n EN ÜSTÜNE (di?er using'lerden sonra) ekleyin:

```csharp
using CoreBuilder.Infrastructure.Caching;
using CoreBuilder.Infrastructure.Middleware;
using CoreBuilder.Infrastructure.Security;
using StackExchange.Redis;
```

---

## Ad?m 2: Cache Sistemi Ekle

`builder.Services.AddMemoryCache();` sat?r?ndan SONRA ekleyin:

```csharp
// ???????????????????????????????????????????????????????????
// CACHING CONFIGURATION (Redis or Memory)
// ???????????????????????????????????????????????????????????
var useRedis = builder.Configuration.GetValue<bool>("CacheSettings:UseRedis", false);
var defaultExpiration = TimeSpan.FromMinutes(
    builder.Configuration.GetValue<int>("CacheSettings:DefaultExpirationMinutes", 30));

if (useRedis)
{
    try
    {
        var redisConnection = builder.Configuration.GetConnectionString("RedisConnection");
        var redis = ConnectionMultiplexer.Connect(redisConnection!);
        builder.Services.AddSingleton<IConnectionMultiplexer>(redis);
        builder.Services.AddSingleton<ICacheService>(sp =>
            new RedisCacheService(
                sp.GetRequiredService<IConnectionMultiplexer>(),
                sp.GetRequiredService<ILogger<RedisCacheService>>(),
                defaultExpiration));
        Log.Information("?? Redis Cache enabled: {Connection}", redisConnection);
    }
    catch (Exception ex)
    {
        Log.Warning(ex, "?? Redis connection failed, falling back to Memory Cache");
        builder.Services.AddSingleton<ICacheService>(sp =>
            new MemoryCacheService(
                sp.GetRequiredService<IMemoryCache>(),
                sp.GetRequiredService<ILogger<MemoryCacheService>>(),
                defaultExpiration));
    }
}
else
{
    builder.Services.AddSingleton<ICacheService>(sp =>
        new MemoryCacheService(
            sp.GetRequiredService<IMemoryCache>(),
            sp.GetRequiredService<ILogger<MemoryCacheService>>(),
            defaultExpiration));
    Log.Information("?? Memory Cache enabled");
}
```

---

## Ad?m 3: Enhanced Security

Mevcut JWT kodunu ?u sat?rlarla DE???T?R?N:

```csharp
// ESKI KOD (S?L):
// var jwtKey = builder.Configuration["Jwt:Key"] ?? "YourSecretKeyHere123456789";
// builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
//     .AddJwtBearer(options => { ... });
// builder.Services.AddAuthorization();

// YEN? KOD (EKLE):
builder.Services.AddJwtAuthentication(builder.Configuration);
builder.Services.AddAuthorization();
builder.Services.AddCorsPolicies(builder.Configuration);
Log.Information("?? Security (JWT + CORS) configured");
```

---

## Ad?m 4: Middleware Pipeline

`var app = builder.Build();` sat?r?ndan SONRA, `if (app.Environment.IsDevelopment())` sat?r?ndan ÖNCE ekleyin:

```csharp
// ???????????????????????????????????????????????????????????
// MIDDLEWARE PIPELINE
// ???????????????????????????????????????????????????????????

// 1. Global exception handling (EN ÖNCE OLMALI)
app.UseMiddleware<GlobalExceptionMiddleware>();
```

`app.UseSerilogRequestLogging();` sat?r?ndan SONRA ekleyin:

```csharp
// 2. Request logging
app.UseMiddleware<RequestLoggingMiddleware>();
```

`app.UseRouting();` sat?r?ndan SONRA ekleyin:

```csharp
// 3. CORS
app.UseCors(CorsExtensions.DefaultPolicyName);
```

---

## Ad?m 5: Swagger Güncelle

`app.UseSwagger();` ve `app.UseSwaggerUI();` sat?rlar?n? ?ununla de?i?tirin:

```csharp
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "CoreBuilder API v1");
    c.RoutePrefix = "swagger";
});
```

---

## Ad?m 6: Seed Data Log Güncelle

Seed data blo?unun sonundaki log sat?rlar?n? ?ununla de?i?tirin:

```csharp
Log.Information("???????????????????????????????????");
Log.Information("?? CoreBuilder haz?r!");
Log.Information("?? https://localhost:5001");
Log.Information("?? admin / Admin123!");
Log.Information("?? Cache: {CacheType}", useRedis ? "Redis" : "Memory");
Log.Information("???  YÖK: http://yok.localhost:5001");
Log.Information("?? Demo: http://demo.localhost:5001");
Log.Information("?? S?navKurs: http://sinavkurs.localhost:5001");
Log.Information("???????????????????????????????????");
```

---

## ? Kontrol Listesi

- [ ] Using'ler eklendi
- [ ] Cache sistemi eklendi
- [ ] JWT kodu de?i?tirildi
- [ ] CORS eklendi
- [ ] Middleware'ler eklendi
- [ ] Swagger güncellendi
- [ ] Log mesajlar? güncellendi

---

## ?? Test Et

```powershell
cd CoreBuilder.Admin
dotnet build
dotnet run
```

Beklenen ç?kt?:

```
?? CoreBuilder ba?lat?l?yor...
?? Memory Cache enabled
?? Security (JWT + CORS) configured
? 3 tema eklendi
? Admin: admin / Admin123!
?? CoreBuilder haz?r!
?? Cache: Memory
```
