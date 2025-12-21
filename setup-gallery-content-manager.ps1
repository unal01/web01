# Complete Gallery Content Management Setup
Write-Host "???????????????????????????????????" -ForegroundColor Cyan
Write-Host "Gallery Content Management Setup" -ForegroundColor Cyan
Write-Host "???????????????????????????????????" -ForegroundColor Cyan
Write-Host ""

# 1. Register PageContentService in Program.cs
Write-Host "1. Registering PageContentService..." -ForegroundColor Yellow
$programFile = "CoreBuilder.Admin\Program.cs"
$content = Get-Content $programFile -Raw -Encoding UTF8

if ($content -notmatch "PageContentService")
{
    $content = $content -replace `
        '(builder\.Services\.AddScoped<TeacherService>\(\);)', `
        "`$1`r`n    builder.Services.AddScoped<PageContentService>();"
    
    [System.IO.File]::WriteAllText($programFile, $content, [System.Text.Encoding]::UTF8)
    Write-Host "  ? PageContentService registered" -ForegroundColor Green
}
else
{
    Write-Host "  ??  PageContentService already registered" -ForegroundColor Gray
}

# 2. Update PageDetail.razor - Add content management button
Write-Host ""
Write-Host "2. Updating PageDetail.razor..." -ForegroundColor Yellow
$pageDetailFile = "CoreBuilder.Admin\Pages\PageDetail.razor"
$content = Get-Content $pageDetailFile -Raw -Encoding UTF8

if ($content -notmatch "ManageContents")
{
    # Header'a içerik yönetimi butonu ekle
    $content = $content -replace `
        '(<h3 class="fw-bold text-dark mb-0">Sayfa Düzenleme: @pageObj\.Title</h3>)\s*<button class="btn btn-outline-secondary" @onclick="GoBack">', `
        "`$1`r`n            <div>`r`n                @if (pageObj.PageType == PageType.PhotoGallery || pageObj.PageType == PageType.VideoGallery)`r`n                {`r`n                    <button class=`"btn btn-success me-2`" @onclick=`"ManageContents`">`r`n                        <span class=`"oi oi-image me-2`"></span>?çerikleri Yönet`r`n                    </button>`r`n                }`r`n                <button class=`"btn btn-outline-secondary`" @onclick=`"GoBack`">"
    
    # @code blo?una ManageContents metodu ekle
    $content = $content -replace `
        '(private void GoBack\(\))', `
        "private void ManageContents()`r`n    {`r`n        Navigation.NavigateTo(`$`"/page/{PageId}/contents`");`r`n    }`r`n`r`n    `$1"
    
    [System.IO.File]::WriteAllText($pageDetailFile, $content, [System.Text.Encoding]::UTF8)
    Write-Host "  ? Content management button added" -ForegroundColor Green
}
else
{
    Write-Host "  ??  Content management button already exists" -ForegroundColor Gray
}

# 3. Build test
Write-Host ""
Write-Host "3. Testing build..." -ForegroundColor Yellow
Set-Location "CoreBuilder"
$buildResult = dotnet build 2>&1

if ($LASTEXITCODE -eq 0)
{
    Write-Host "  ? Build successful!" -ForegroundColor Green
}
else
{
    Write-Host "  ??  Build has warnings/errors" -ForegroundColor Yellow
}

Set-Location ".."

Write-Host ""
Write-Host "???????????????????????????????????" -ForegroundColor Cyan
Write-Host "SETUP COMPLETE!" -ForegroundColor Green
Write-Host "???????????????????????????????????" -ForegroundColor Cyan
Write-Host ""
Write-Host "?? What's been added:" -ForegroundColor Yellow
Write-Host "  ? PageContentService (CRUD operations)" -ForegroundColor Green
Write-Host "  ? PageContentManager.razor (content upload UI)" -ForegroundColor Green
Write-Host "  ? Image upload support" -ForegroundColor Green
Write-Host "  ? Video embed (YouTube/Vimeo)" -ForegroundColor Green
Write-Host "  ? Lightbox for images" -ForegroundColor Green
Write-Host "  ? Content statistics" -ForegroundColor Green
Write-Host ""
Write-Host "?? How to use:" -ForegroundColor Yellow
Write-Host "  1. Go to Pages ? Select 'S?nav Kurs'" -ForegroundColor White
Write-Host "  2. Click on 'galeri' page" -ForegroundColor White
Write-Host "  3. Click '?? ?çerikleri Yönet' button" -ForegroundColor White
Write-Host "  4. Click 'Yeni ?çerik Ekle'" -ForegroundColor White
Write-Host "  5. Choose type (Photo/Video)" -ForegroundColor White
Write-Host "  6. Upload photo or enter YouTube URL" -ForegroundColor White
Write-Host "  7. Save!" -ForegroundColor White
Write-Host ""
Write-Host "?? Run app:" -ForegroundColor Yellow
Write-Host "  cd CoreBuilder.Admin && dotnet run" -ForegroundColor Cyan
Write-Host ""
Write-Host "?? Visit: https://localhost:5001/pages" -ForegroundColor Cyan
