# CoreBuilder - Git Commit Haz?rl?k

Write-Host "???????????????????????????????????" -ForegroundColor Cyan
Write-Host "Git Commit Haz?rl?k" -ForegroundColor Cyan
Write-Host "???????????????????????????????????" -ForegroundColor Cyan
Write-Host ""

# 1. Restore packages
Write-Host "?? Paketler yükleniyor..." -ForegroundColor Yellow
Set-Location "CoreBuilder"
dotnet restore
if ($LASTEXITCODE -ne 0) {
    Write-Host "? Restore ba?ar?s?z!" -ForegroundColor Red
    exit 1
}
Write-Host "? Paketler yüklendi" -ForegroundColor Green
Write-Host ""

# 2. Build
Write-Host "?? Build yap?l?yor..." -ForegroundColor Yellow
dotnet build --no-restore
if ($LASTEXITCODE -ne 0) {
    Write-Host "? Build ba?ar?s?z!" -ForegroundColor Red
    exit 1
}
Write-Host "? Build ba?ar?l?" -ForegroundColor Green
Write-Host ""

# 3. Test project update
Write-Host "?? Test projesi güncelleniyor..." -ForegroundColor Yellow
Set-Location "..\CoreBuilder.Tests"

if (Test-Path "CoreBuilder.Tests.Updated.csproj") {
    if (Test-Path "CoreBuilder.Tests.csproj") {
        Remove-Item "CoreBuilder.Tests.csproj" -Force
    }
    Rename-Item "CoreBuilder.Tests.Updated.csproj" "CoreBuilder.Tests.csproj"
    Write-Host "? Test projesi güncellendi" -ForegroundColor Green
} else {
    Write-Host "??  Test projesi zaten güncel" -ForegroundColor Gray
}
Write-Host ""

# 4. Test restore & build
Write-Host "?? Test paketleri yükleniyor..." -ForegroundColor Yellow
dotnet restore
if ($LASTEXITCODE -ne 0) {
    Write-Host "? Test restore ba?ar?s?z!" -ForegroundColor Red
    exit 1
}
Write-Host "? Test paketleri yüklendi" -ForegroundColor Green
Write-Host ""

Write-Host "?? Test projesi build ediliyor..." -ForegroundColor Yellow
dotnet build --no-restore
if ($LASTEXITCODE -ne 0) {
    Write-Host "? Test build ba?ar?s?z!" -ForegroundColor Red
    exit 1
}
Write-Host "? Test build ba?ar?l?" -ForegroundColor Green
Write-Host ""

# 5. Run tests
Write-Host "?? Testler çal??t?r?l?yor..." -ForegroundColor Yellow
dotnet test --no-build --logger "console;verbosity=minimal"
$testResult = $LASTEXITCODE
Write-Host ""

if ($testResult -eq 0) {
    Write-Host "? Tüm testler geçti!" -ForegroundColor Green
} else {
    Write-Host "??  Baz? testler ba?ar?s?z (devam ediliyor)" -ForegroundColor Yellow
}
Write-Host ""

# 6. Git status
Set-Location ".."
Write-Host "?? Git durumu:" -ForegroundColor Yellow
git status --short
Write-Host ""

# 7. Summary
Write-Host "???????????????????????????????????" -ForegroundColor Cyan
Write-Host "HAZIR!" -ForegroundColor Green
Write-Host "???????????????????????????????????" -ForegroundColor Cyan
Write-Host ""
Write-Host "SONRAK? ADIMLAR:" -ForegroundColor Yellow
Write-Host ""
Write-Host "1. De?i?iklikleri kontrol et:" -ForegroundColor White
Write-Host "   git status" -ForegroundColor Cyan
Write-Host ""
Write-Host "2. Commit yap:" -ForegroundColor White
Write-Host "   git add ." -ForegroundColor Cyan
Write-Host "   git commit -m `"feat: Add Redis caching, global error handler, enhanced security & testing`"" -ForegroundColor Cyan
Write-Host ""
Write-Host "3. GitHub'a push et:" -ForegroundColor White
Write-Host "   git push origin main" -ForegroundColor Cyan
Write-Host ""
Write-Host "4. Uygulamay? çal??t?r:" -ForegroundColor White
Write-Host "   cd CoreBuilder.Admin" -ForegroundColor Cyan
Write-Host "   dotnet run" -ForegroundColor Cyan
Write-Host ""
Write-Host "???????????????????????????????????" -ForegroundColor Cyan
