# CoreBuilder - Build & Test Script
Write-Host "???????????????????????????????????" -ForegroundColor Cyan
Write-Host "CoreBuilder - Build & Test" -ForegroundColor Cyan
Write-Host "???????????????????????????????????" -ForegroundColor Cyan
Write-Host ""

# 1. Restore packages
Write-Host "?? Restoring NuGet packages..." -ForegroundColor Yellow
Write-Host ""

Write-Host "  CoreBuilder project..." -ForegroundColor Gray
Set-Location "CoreBuilder"
dotnet restore
if ($LASTEXITCODE -ne 0) {
    Write-Host "? CoreBuilder restore failed!" -ForegroundColor Red
    exit 1
}
Write-Host "  ? CoreBuilder restored" -ForegroundColor Green
Write-Host ""

Write-Host "  CoreBuilder.Tests project..." -ForegroundColor Gray
Set-Location "..\CoreBuilder.Tests"

# Test projesinde eski csproj dosyas?n? yeni ile de?i?tir
if (Test-Path "CoreBuilder.Tests.Updated.csproj") {
    Write-Host "  Updating test project file..." -ForegroundColor Gray
    Remove-Item "CoreBuilder.Tests.csproj" -ErrorAction SilentlyContinue
    Rename-Item "CoreBuilder.Tests.Updated.csproj" "CoreBuilder.Tests.csproj"
}

dotnet restore
if ($LASTEXITCODE -ne 0) {
    Write-Host "? CoreBuilder.Tests restore failed!" -ForegroundColor Red
    exit 1
}
Write-Host "  ? CoreBuilder.Tests restored" -ForegroundColor Green
Write-Host ""

Write-Host "  CoreBuilder.Admin project..." -ForegroundColor Gray
Set-Location "..\CoreBuilder.Admin"
dotnet restore
if ($LASTEXITCODE -ne 0) {
    Write-Host "? CoreBuilder.Admin restore failed!" -ForegroundColor Red
    exit 1
}
Write-Host "  ? CoreBuilder.Admin restored" -ForegroundColor Green
Write-Host ""

# 2. Build projects
Write-Host "?? Building projects..." -ForegroundColor Yellow
Write-Host ""

Write-Host "  Building CoreBuilder..." -ForegroundColor Gray
Set-Location "..\CoreBuilder"
dotnet build --no-restore
if ($LASTEXITCODE -ne 0) {
    Write-Host "? CoreBuilder build failed!" -ForegroundColor Red
    Write-Host ""
    Write-Host "Hata Çözümleri:" -ForegroundColor Yellow
    Write-Host "1. Program.cs'i güncellediniz mi? Bkz: PROGRAM_CS_UPDATE_GUIDE.md" -ForegroundColor White
    Write-Host "2. Infrastructure klasörü var m??" -ForegroundColor White
    Write-Host "3. dotnet clean && dotnet restore deneyin" -ForegroundColor White
    exit 1
}
Write-Host "  ? CoreBuilder built successfully" -ForegroundColor Green
Write-Host ""

Write-Host "  Building CoreBuilder.Tests..." -ForegroundColor Gray
Set-Location "..\CoreBuilder.Tests"
dotnet build --no-restore
if ($LASTEXITCODE -ne 0) {
    Write-Host "? CoreBuilder.Tests build failed!" -ForegroundColor Red
    exit 1
}
Write-Host "  ? CoreBuilder.Tests built successfully" -ForegroundColor Green
Write-Host ""

# 3. Run tests
Write-Host "?? Running tests..." -ForegroundColor Yellow
Write-Host ""

dotnet test --no-build --logger "console;verbosity=normal"
$testExitCode = $LASTEXITCODE

Write-Host ""
if ($testExitCode -eq 0) {
    Write-Host "? All tests passed!" -ForegroundColor Green
} else {
    Write-Host "??  Some tests failed (Exit code: $testExitCode)" -ForegroundColor Yellow
}
Write-Host ""

# 4. Build admin
Write-Host "?? Building CoreBuilder.Admin..." -ForegroundColor Yellow
Set-Location "..\CoreBuilder.Admin"
dotnet build --no-restore
if ($LASTEXITCODE -ne 0) {
    Write-Host "? CoreBuilder.Admin build failed!" -ForegroundColor Red
    exit 1
}
Write-Host "? CoreBuilder.Admin built successfully" -ForegroundColor Green
Write-Host ""

# 5. Summary
Write-Host "???????????????????????????????????" -ForegroundColor Cyan
Write-Host "BUILD SUMMARY" -ForegroundColor Cyan
Write-Host "???????????????????????????????????" -ForegroundColor Cyan
Write-Host ""
Write-Host "? CoreBuilder - OK" -ForegroundColor Green
Write-Host "? CoreBuilder.Tests - OK" -ForegroundColor Green
if ($testExitCode -eq 0) {
    Write-Host "? Tests - PASSED" -ForegroundColor Green
} else {
    Write-Host "??  Tests - SOME FAILED" -ForegroundColor Yellow
}
Write-Host "? CoreBuilder.Admin - OK" -ForegroundColor Green
Write-Host ""

Write-Host "NEXT STEPS:" -ForegroundColor Yellow
Write-Host "1. Check PROGRAM_CS_UPDATE_GUIDE.md if build failed" -ForegroundColor White
Write-Host "2. Run the application:" -ForegroundColor White
Write-Host "   cd CoreBuilder.Admin" -ForegroundColor Cyan
Write-Host "   dotnet run" -ForegroundColor Cyan
Write-Host ""
Write-Host "3. Access the application:" -ForegroundColor White
Write-Host "   https://localhost:5001" -ForegroundColor Cyan
Write-Host "   Admin: admin / Admin123!" -ForegroundColor Cyan
Write-Host ""

Set-Location "..\"
Write-Host "???????????????????????????????????" -ForegroundColor Cyan
