# Quick Fix for PageContentManager.razor Build Errors
Write-Host "Fixing PageContentManager.razor..." -ForegroundColor Yellow

$file = "CoreBuilder.Admin\Pages\PageContentManager.razor"
$content = Get-Content $file -Raw -Encoding UTF8

# 1. Add System.IO using
if ($content -notmatch "using System\.IO;")
{
    $content = $content -replace `
        '(@inject NavigationManager Navigation)', `
        "using System.IO;`r`n`$1"
    
    Write-Host "  ? Added 'using System.IO;'" -ForegroundColor Green
}

# 2. Fix Path.GetExtension (use full namespace)
$content = $content -replace `
    'var extension = Path\.GetExtension\(file\.Name\)', `
    'var extension = System.IO.Path.GetExtension(file.Name)'

# 3. Fix ImageEntry property access
$content = $content -replace `
    'var imageEntry = await ImageService\.UploadImageAsync\(file, currentPage!\.TenantId\);\s*newMediaUrl = imageEntry\.FilePath;\s*newThumbnailUrl = imageEntry\.FilePath;', `
    'var filePath = await ImageService.UploadImageAsync(file, currentPage!.TenantId);
            newMediaUrl = filePath;
            newThumbnailUrl = filePath;'

[System.IO.File]::WriteAllText($file, $content, [System.Text.Encoding]::UTF8)
Write-Host "  ? PageContentManager.razor fixed!" -ForegroundColor Green

Write-Host ""
Write-Host "Building..." -ForegroundColor Yellow
Set-Location "CoreBuilder"
dotnet build 2>&1 | Out-Null

if ($LASTEXITCODE -eq 0)
{
    Write-Host "? Build successful!" -ForegroundColor Green
}
else
{
    Write-Host "??  Build still has errors, checking..." -ForegroundColor Yellow
    dotnet build
}

Set-Location ".."
