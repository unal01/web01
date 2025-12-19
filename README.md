# CoreBuilder CMS

Modern Multi-Tenant CMS Platform

## Kurulum

### Gereksinimler:
- .NET 8.0 SDK
- (Opsiyonel) SQL Server

### Baslatma:

```powershell
# Bagimliliklari yukle
dotnet restore

# Admin Panel'i calistir
cd CoreBuilder.Admin
dotnet run
```

## Erisim

- Admin Panel: https://localhost:5001
- Demo Site: https://localhost:5001/demo
- Swagger: https://localhost:5001/api-docs
- GraphQL: https://localhost:5001/graphql

## Varsayilan Giris

- Kullanici: admin
- Sifre: Admin123!

## Ozellikler

- Multi-Tenant Mimari
- Slider Yonetimi
- Duyuru Sistemi
- Dinamik Sayfa Olusturma
- Tema Sistemi
- Kullanici Yonetimi
- Medya Galerisi
- GraphQL API
- SignalR Real-time
- Rate Limiting
- JWT Authentication

## Proje Yapisi

```
CoreBuilder/
â”œâ”€â”€ CoreBuilder/           # Core Library
â”œâ”€â”€ CoreBuilder.Admin/     # Blazor Admin Panel
â””â”€â”€ README.md
```

## Teknolojiler

- ASP.NET Core 8.0
- Blazor Server
- Entity Framework Core
- SignalR
- GraphQL (HotChocolate)
- JWT Bearer
- Serilog
