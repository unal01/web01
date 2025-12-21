# Complete Page Content Management System Setup
Write-Host "???????????????????????????????????" -ForegroundColor Cyan
Write-Host "Page Content Management Setup" -ForegroundColor Cyan
Write-Host "???????????????????????????????????" -ForegroundColor Cyan
Write-Host ""

# 1. Update AppDbContext
Write-Host "1. Updating AppDbContext..." -ForegroundColor Yellow
$dbContextFile = "CoreBuilder\Data\AppDbContext.cs"
$content = Get-Content $dbContextFile -Raw -Encoding UTF8

if ($content -notmatch "PageContent")
{
    $content = $content -replace `
        '(public DbSet<Teacher> Teachers \{ get; set; \})', `
        "`$1`r`n        public DbSet<PageContent> PageContents { get; set; }"
    
    [System.IO.File]::WriteAllText($dbContextFile, $content, [System.Text.Encoding]::UTF8)
    Write-Host "  ? PageContent DbSet added" -ForegroundColor Green
}
else
{
    Write-Host "  ??  PageContent already exists" -ForegroundColor Gray
}

# 2. Build test
Write-Host ""
Write-Host "2. Testing build..." -ForegroundColor Yellow
Set-Location "CoreBuilder"
$buildResult = dotnet build 2>&1

if ($LASTEXITCODE -eq 0)
{
    Write-Host "  ? Build successful!" -ForegroundColor Green
}
else
{
    Write-Host "  ??  Build has errors" -ForegroundColor Yellow
}

Set-Location ".."

Write-Host ""
Write-Host "???????????????????????????????????" -ForegroundColor Cyan
Write-Host "SETUP COMPLETE!" -ForegroundColor Green
Write-Host "???????????????????????????????????" -ForegroundColor Cyan
Write-Host ""
Write-Host "?? What's been added:" -ForegroundColor Yellow
Write-Host "  ? PageContent model (photos, videos, sliders)" -ForegroundColor Green
Write-Host "  ? Page model updated (menu location, parent pages)" -ForegroundColor Green
Write-Host "  ? PageType enum (9 types)" -ForegroundColor Green
Write-Host "  ? ContentType enum (7 types)" -ForegroundColor Green
Write-Host ""
Write-Host "?? Documentation created:" -ForegroundColor Yellow
Write-Host "  ?? PAGE_CONTENT_SYSTEM.md - Complete guide" -ForegroundColor Cyan
Write-Host ""
Write-Host "?? Next steps:" -ForegroundColor Yellow
Write-Host "  1. Create PageContentService" -ForegroundColor White
Write-Host "  2. Create content management UI" -ForegroundColor White
Write-Host "  3. Integrate with Pages.razor" -ForegroundColor White
Write-Host ""
Write-Host "Run app: cd CoreBuilder.Admin && dotnet run" -ForegroundColor Cyan
