# CoreBuilder - Proje Transfer Scripti
Write-Host "CoreBuilder Transfer Hazirlik" -ForegroundColor Cyan
Write-Host "==============================" -ForegroundColor Cyan

# 1. Gereksiz dosyalari temizle
Write-Host ""
Write-Host "Gereksiz dosyalar temizleniyor..." -ForegroundColor Yellow

$foldersToDelete = @("bin", "obj", ".vs", "logs", "packages")
foreach ($folder in $foldersToDelete) {
    Get-ChildItem -Path . -Directory -Recurse -Filter $folder -ErrorAction SilentlyContinue | 
    Remove-Item -Recurse -Force -ErrorAction SilentlyContinue
}

Write-Host "Temizlik tamamlandi" -ForegroundColor Green

# 2. .gitignore olustur
Write-Host ""
Write-Host ".gitignore olusturuluyor..." -ForegroundColor Yellow

$gitignoreContent = @"
# Build results
[Bb]in/
[Oo]bj/
[Ll]og/
[Ll]ogs/

# Visual Studio
.vs/
*.user
*.suo
*.userosscache
*.sln.docstates

# .NET Core
project.lock.json
project.fragment.lock.json
artifacts/

# Others
*.log
*.bak
*.cache
"@

$gitignoreContent | Out-File -FilePath ".gitignore" -Encoding utf8
Write-Host ".gitignore olusturuldu" -ForegroundColor Green

# 3. README olustur
Write-Host ""
Write-Host "README.md olusturuluyor..." -ForegroundColor Yellow

$readmeContent = @"
# CoreBuilder CMS

Modern Multi-Tenant CMS Platform

## Kurulum

### Gereksinimler:
- .NET 8.0 SDK
- (Opsiyonel) SQL Server

### Baslatma:

``````powershell
# Bagimliliklari yukle
dotnet restore

# Admin Panel'i calistir
cd CoreBuilder.Admin
dotnet run
``````

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

``````
CoreBuilder/
├── CoreBuilder/           # Core Library
├── CoreBuilder.Admin/     # Blazor Admin Panel
└── README.md
``````

## Teknolojiler

- ASP.NET Core 8.0
- Blazor Server
- Entity Framework Core
- SignalR
- GraphQL (HotChocolate)
- JWT Bearer
- Serilog
"@

$readmeContent | Out-File -FilePath "README.md" -Encoding utf8
Write-Host "README.md olusturuldu" -ForegroundColor Green

# 4. Git baslat
Write-Host ""
Write-Host "Git repository baslatiliyor..." -ForegroundColor Yellow

if (Test-Path ".git") {
    Write-Host "Git repository zaten var" -ForegroundColor Yellow
} else {
    git init
    git add .
    git commit -m "Initial commit - CoreBuilder CMS"
    Write-Host "Git repository olusturuldu" -ForegroundColor Green
}

# 5. ZIP olustur
Write-Host ""
Write-Host "Yedek ZIP olusturuluyor..." -ForegroundColor Yellow

$zipPath = "D:\CoreBuilder-Backup-$(Get-Date -Format 'yyyyMMdd-HHmmss').zip"
Compress-Archive -Path ".\*" -DestinationPath $zipPath -Force

Write-Host "ZIP olusturuldu: $zipPath" -ForegroundColor Green

# 6. Ozet
Write-Host ""
Write-Host "==============================" -ForegroundColor Cyan
Write-Host "HAZIRLIK TAMAMLANDI!" -ForegroundColor Green
Write-Host "==============================" -ForegroundColor Cyan

Write-Host ""
Write-Host "YAPMANIZ GEREKENLER:" -ForegroundColor Yellow
Write-Host ""
Write-Host "1. GitHub'da yeni repository olustur:" -ForegroundColor White
Write-Host "   https://github.com/new" -ForegroundColor Cyan
Write-Host ""
Write-Host "2. Bu komutu calistir:" -ForegroundColor White
Write-Host "   git remote add origin https://github.com/KULLANICI_ADINIZ/corebuilder.git" -ForegroundColor Cyan
Write-Host "   git branch -M main" -ForegroundColor Cyan
Write-Host "   git push -u origin main" -ForegroundColor Cyan
Write-Host ""
Write-Host "3. Evdeki bilgisayarda:" -ForegroundColor White
Write-Host "   git clone https://github.com/KULLANICI_ADINIZ/corebuilder.git" -ForegroundColor Cyan
Write-Host "   cd corebuilder/CoreBuilder.Admin" -ForegroundColor Cyan
Write-Host "   dotnet run" -ForegroundColor Cyan
Write-Host ""
Write-Host "ALTERNATIF: ZIP dosyasini USB'ye kopyala:" -ForegroundColor Yellow
Write-Host "   $zipPath" -ForegroundColor Cyan
Write-Host ""
Write-Host "==============================" -ForegroundColor Cyan
