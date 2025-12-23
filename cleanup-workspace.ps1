# CoreBuilder - Workspace Temizleme
Write-Host "?? Gereksiz dosyalar siliniyor..." -ForegroundColor Yellow

# 1. Build artifacts
Write-Host "`n?? Build artifacts siliniyor..."
$buildFolders = @("bin", "obj", "packages", "logs")
foreach ($folder in $buildFolders) {
    Get-ChildItem -Path . -Directory -Recurse -Filter $folder -ErrorAction SilentlyContinue | 
    Remove-Item -Recurse -Force -ErrorAction SilentlyContinue
    Write-Host "  ? $folder" -ForegroundColor Green
}

# 2. Eski WebForms dosyalarý
Write-Host "`n??? Eski WebForms dosyalarý siliniyor..."
$oldFiles = @(
    "About.aspx*",
    "Contact.aspx*",
    "Default.aspx*",
    "Global.asax*",
    "Site.Master*",
    "Site.Mobile.Master*",
    "ViewSwitcher.ascx*",
    "*.vb"
)
foreach ($pattern in $oldFiles) {
    Get-ChildItem -Path . -Recurse -Filter $pattern -File -ErrorAction SilentlyContinue | 
    Remove-Item -Force -ErrorAction SilentlyContinue
}
Write-Host "  ? WebForms dosyalarý silindi" -ForegroundColor Green

# 3. Eski klasörler
Write-Host "`n?? Eski klasörler siliniyor..."
$oldFolders = @("App_Start", "Content", "Scripts", "My Project")
foreach ($folder in $oldFolders) {
    if (Test-Path $folder) {
        Remove-Item -Path $folder -Recurse -Force -ErrorAction SilentlyContinue
        Write-Host "  ? $folder" -ForegroundColor Green
    }
}

# 4. Enhanced dosyalarý
Write-Host "`n?? Enhanced duplike dosyalar siliniyor..."
Get-ChildItem -Recurse -Filter "*Enhanced*" -ErrorAction SilentlyContinue | 
Remove-Item -Force -ErrorAction SilentlyContinue
Write-Host "  ? Enhanced dosyalar silindi" -ForegroundColor Green

# 5. Cache dosyalarý
Write-Host "`n?? Cache dosyalarý siliniyor..."
Get-ChildItem -Recurse -Filter "*.cache" -ErrorAction SilentlyContinue | 
Remove-Item -Force -ErrorAction SilentlyContinue
Write-Host "  ? Cache temizlendi" -ForegroundColor Green

# Özet
Write-Host "`n" -NoNewline
Write-Host "================================" -ForegroundColor Cyan
Write-Host "? TEMÝZLEME TAMAMLANDI!" -ForegroundColor Green
Write-Host "================================" -ForegroundColor Cyan

Write-Host "`n?? Workspace boyutu azaltýldý!" -ForegroundColor Yellow
Write-Host ""
Write-Host "?? Þimdi yapmanýz gerekenler:" -ForegroundColor Yellow
Write-Host "  1. dotnet restore              # Paketleri yeniden yükle" -ForegroundColor White
Write-Host "  2. dotnet build                # Projeyi derle" -ForegroundColor White
Write-Host "  3. git add .                   # Deðiþiklikleri ekle" -ForegroundColor White
Write-Host "  4. git commit -m 'Cleanup'     # Commit" -ForegroundColor White
Write-Host "  5. git push                    # GitHub'a gönder" -ForegroundColor White
Write-Host ""
Write-Host "================================" -ForegroundColor Cyan
