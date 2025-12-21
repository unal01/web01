# Update Pages.razor with PageType Selection
Write-Host "Updating Pages.razor with PageType selection..." -ForegroundColor Yellow

$pagesFile = "CoreBuilder.Admin\Pages\Pages.razor"
$content = Get-Content $pagesFile -Raw -Encoding UTF8

# Sayfa türü seçimi eklenmi?mi kontrol et
if ($content -notmatch "selectedPageType")
{
    Write-Host "Adding PageType selection to modal..." -ForegroundColor Cyan
    
    # Modal body'de pageSlug'dan sonra sayfa türü ekle
    $pageTypeHtml = @'

                    <div class="mb-3">
                        <label class="form-label fw-bold" style="font-size: 0.95rem;">Sayfa Türü</label>
                        <select class="form-select" @bind="selectedPageType" style="font-size: 0.95rem;">
                            @foreach (PageType type in Enum.GetValues(typeof(PageType)))
                            {
                                <option value="@type">@type.GetDisplayName()</option>
                            }
                        </select>
                        <small class="text-muted">@selectedPageType.GetDescription()</small>
                    </div>
'@

    $content = $content -replace `
        '(<input class="form-control" @bind="pageSlug" placeholder="iletisim" style="font-size: 0\.95rem;" />)\s*</div>', `
        "`$1</div>$pageTypeHtml"
    
    # @code blo?una selectedPageType ekle
    $content = $content -replace `
        '(private string pageSlug = string\.Empty;)', `
        "`$1`r`n    private PageType selectedPageType = PageType.Standard;"
    
    # SaveNewPage metodunda PageType ekle
    $content = $content -replace `
        '(Slug = pageSlug,)', `
        "`$1`r`n            PageType = selectedPageType,"
    
    # Tabloya sayfa türü sütunu ekle (header)
    $content = $content -replace `
        '(<th style="font-size: 0\.9rem; font-weight: 600;">URL Yolu \(Slug\)</th>)', `
        "`$1`r`n                                    <th style=`"font-size: 0.9rem; font-weight: 600;`">Tür</th>"
    
    # Tabloya sayfa türü sütunu ekle (data)
    $content = $content -replace `
        '(<span class="badge bg-light text-dark border">/@p\.Slug</span>\s*</td>)', `
        "`$1`r`n                                        <td style=`"font-size: 0.9rem;`">`r`n                                            <span class=`"badge bg-info`">`r`n                                                <span class=`"@p.PageType.GetIcon() me-1`"></span>`r`n                                                @p.PageType.GetDisplayName()`r`n                                            </span>`r`n                                        </td>"
    
    [System.IO.File]::WriteAllText($pagesFile, $content, [System.Text.Encoding]::UTF8)
    Write-Host "? Pages.razor updated successfully!" -ForegroundColor Green
}
else
{
    Write-Host "??  PageType selection already exists" -ForegroundColor Gray
}

Write-Host ""
Write-Host "Testing build..." -ForegroundColor Yellow
Set-Location "CoreBuilder"
$buildResult = dotnet build --no-restore 2>&1

if ($LASTEXITCODE -eq 0)
{
    Write-Host "? Build successful!" -ForegroundColor Green
}
else
{
    Write-Host "??  Build failed, check errors" -ForegroundColor Red
}

Set-Location ".."

Write-Host ""
Write-Host "???????????????????????????????????" -ForegroundColor Cyan
Write-Host "DONE!" -ForegroundColor Green
Write-Host "???????????????????????????????????" -ForegroundColor Cyan
Write-Host ""
Write-Host "Now you can:" -ForegroundColor Yellow
Write-Host "1. Run the app: cd CoreBuilder.Admin && dotnet run" -ForegroundColor Cyan
Write-Host "2. Go to /pages" -ForegroundColor Cyan
Write-Host "3. Click 'Yeni Sayfa Ekle'" -ForegroundColor Cyan
Write-Host "4. Select page type (Galeri, Ö?retmen, etc.)" -ForegroundColor Cyan
