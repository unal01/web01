# CoreBuilder - Evde Kurulum Scripti
# Bu scripti proje klasöründe çalıştırın

Write-Host "🏠 CoreBuilder Evde Kurulum" -ForegroundColor Cyan
Write-Host "============================" -ForegroundColor Cyan

# 1. .NET SDK Kontrolü
Write-Host "`n🔍 .NET SDK kontrol ediliyor..." -ForegroundColor Yellow

try {
    $dotnetVersion = dotnet --version
    Write-Host "✅ .NET SDK bulundu: $dotnetVersion" -ForegroundColor Green
} catch {
    Write-Host "❌ .NET SDK bulunamadı!" -ForegroundColor Red
    Write-Host "📥 Lütfen .NET 8.0 SDK indirin:" -ForegroundColor Yellow
    Write-Host "   https://dotnet.microsoft.com/download/dotnet/8.0" -ForegroundColor Cyan
    exit
}

# 2. Bağımlılıkları Yükle
Write-Host "`n📦 Paketler yükleniyor..." -ForegroundColor Yellow

dotnet restore

if ($LASTEXITCODE -eq 0) {
    Write-Host "✅ Paketler başarıyla yüklendi" -ForegroundColor Green
} else {
    Write-Host "❌ Paket yükleme hatası!" -ForegroundColor Red
    exit
}

# 3. Build
Write-Host "`n🔨 Proje build ediliyor..." -ForegroundColor Yellow

dotnet build --no-restore

if ($LASTEXITCODE -eq 0) {
    Write-Host "✅ Build başarılı" -ForegroundColor Green
} else {
    Write-Host "❌ Build hatası!" -ForegroundColor Red
    exit
}

# 4. Bilgilendirme
Write-Host "`n" -NoNewline
Write-Host "============================" -ForegroundColor Cyan
Write-Host "✅ KURULUM TAMAMLANDI!" -ForegroundColor Green
Write-Host "============================" -ForegroundColor Cyan

Write-Host "`n🚀 Projeyi Başlatmak İçin:" -ForegroundColor Yellow
Write-Host ""
Write-Host "cd CoreBuilder.Admin" -ForegroundColor Cyan
Write-Host "dotnet run" -ForegroundColor Cyan
Write-Host ""
Write-Host "📍 Erişim Adresleri:" -ForegroundColor Yellow
Write-Host "   • Admin Panel: https://localhost:5001" -ForegroundColor White
Write-Host "   • Demo Site: https://localhost:5001/demo" -ForegroundColor White
Write-Host "   • Swagger: https://localhost:5001/api-docs" -ForegroundColor White
Write-Host ""
Write-Host "👤 Giriş Bilgileri:" -ForegroundColor Yellow
Write-Host "   • Kullanıcı: admin" -ForegroundColor White
Write-Host "   • Şifre: Admin123!" -ForegroundColor White
Write-Host ""
Write-Host "============================" -ForegroundColor Cyan

# 5. Otomatik başlatma sorgusu
Write-Host ""
$response = Read-Host "Şimdi başlatmak ister misiniz? (E/H)"

if ($response -eq "E" -or $response -eq "e") {
    Write-Host "`n🚀 Proje başlatılıyor..." -ForegroundColor Green
    cd CoreBuilder.Admin
    dotnet run
}
