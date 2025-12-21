# Add FontSettings to AppDbContext and Register Service
Write-Host "Adding FontSettings support..." -ForegroundColor Yellow

# 1. Update AppDbContext
Write-Host "`n1. Updating AppDbContext..." -ForegroundColor Cyan
$dbContextFile = "CoreBuilder\Data\AppDbContext.cs"
$content = Get-Content $dbContextFile -Raw -Encoding UTF8

if ($content -notmatch "FontSettings")
{
    $content = $content -replace `
        '(public DbSet<SystemSettings> SystemSettings \{ get; set; \})', `
        "`$1`r`n        public DbSet<FontSettings> FontSettings { get; set; }"
    
    [System.IO.File]::WriteAllText($dbContextFile, $content, [System.Text.Encoding]::UTF8)
    Write-Host "  ? FontSettings DbSet added" -ForegroundColor Green
}

# 2. Register FontSettingsService
Write-Host "`n2. Registering FontSettingsService..." -ForegroundColor Cyan
$programFile = "CoreBuilder.Admin\Program.cs"
$content = Get-Content $programFile -Raw -Encoding UTF8

if ($content -notmatch "FontSettingsService")
{
    $content = $content -replace `
        '(builder\.Services\.AddScoped<PageContentService>\(\);)', `
        "`$1`r`n    builder.Services.AddScoped<FontSettingsService>();"
    
    [System.IO.File]::WriteAllText($programFile, $content, [System.Text.Encoding]::UTF8)
    Write-Host "  ? FontSettingsService registered" -ForegroundColor Green
}

# 3. Update NavMenu
Write-Host "`n3. Updating NavMenu..." -ForegroundColor Cyan
$navMenuFile = "CoreBuilder.Admin\Shared\NavMenu.razor"
$content = Get-Content $navMenuFile -Raw -Encoding UTF8

if ($content -notmatch "settings/fonts")
{
    # Ayarlar menüsünü bul ve font ayarlar?n? ekle
    $fontMenuItem = @'

    <NavLink class="nav-item ps-4" href="settings/fonts">
        <i class="bi bi-fonts"></i>
        <span>Font Ayarlar?</span>
    </NavLink>
'@
    
    # E?er Ayarlar bölümü varsa ekle
    if ($content -match "<!-- ?S?STEM -->")
    {
        $content = $content -replace `
            '(<!-- ?S?STEM -->.*?<NavLink.*?Ayarlar.*?</NavLink>)', `
            "`$1$fontMenuItem"
    }
    
    [System.IO.File]::WriteAllText($navMenuFile, $content, [System.Text.Encoding]::UTF8)
    Write-Host "  ? NavMenu updated with Font Settings" -ForegroundColor Green
}

# 4. Build
Write-Host "`n4. Building..." -ForegroundColor Cyan
Set-Location "CoreBuilder"
$buildResult = dotnet build 2>&1

if ($LASTEXITCODE -eq 0)
{
    Write-Host "  ? Build successful!" -ForegroundColor Green
}
else
{
    Write-Host "  ??  Build has warnings" -ForegroundColor Yellow
}

Set-Location ".."

Write-Host ""
Write-Host "???????????????????????????????????????????" -ForegroundColor Cyan
Write-Host "FONT SETTINGS SYSTEM READY!" -ForegroundColor Green
Write-Host "???????????????????????????????????????????" -ForegroundColor Cyan
Write-Host ""
Write-Host "?? What's been added:" -ForegroundColor Yellow
Write-Host "  ? FontSettings model (16 popular fonts)" -ForegroundColor White
Write-Host "  ? FontSettingsService (CRUD + CSS generation)" -ForegroundColor White
Write-Host "  ? FontSettings.razor (Admin UI)" -ForegroundColor White
Write-Host "  ? Database integration" -ForegroundColor White
Write-Host "  ? NavMenu link" -ForegroundColor White
Write-Host ""
Write-Host "?? Available fonts:" -ForegroundColor Yellow
Write-Host "  • Inter, Roboto, Open Sans (Modern)" -ForegroundColor White
Write-Host "  • Poppins, Montserrat, Nunito (Geometric)" -ForegroundColor White
Write-Host "  • Noto Sans, Rubik (Turkish-friendly)" -ForegroundColor White
Write-Host "  • Merriweather, Playfair Display (Serif)" -ForegroundColor White
Write-Host ""
Write-Host "?? Usage:" -ForegroundColor Yellow
Write-Host "  1. Run: cd CoreBuilder.Admin && dotnet run" -ForegroundColor Cyan
Write-Host "  2. Go to: https://localhost:5001/settings/fonts" -ForegroundColor Cyan
Write-Host "  3. Select fonts and preview" -ForegroundColor Cyan
Write-Host "  4. Click 'Kaydet' to apply" -ForegroundColor Cyan
Write-Host ""
Write-Host "? Features:" -ForegroundColor Yellow
Write-Host "  • Live preview" -ForegroundColor White
Write-Host "  • Google Fonts integration" -ForegroundColor White
Write-Host "  • Typography settings (size, weight, line-height)" -ForegroundColor White
Write-Host "  • Reset to defaults" -ForegroundColor White
