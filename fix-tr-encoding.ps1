# Fix Turkish encoding issues by rewriting known-garbled files as UTF-8
# NOTE: When a file is already garbled (mojibake), rewriting alone won't restore characters.
# We fix by replacing common mojibake sequences -> correct Turkish chars, then write UTF-8.

$ErrorActionPreference = 'Stop'

function Fix-Text($text) {
    $map = @{
        'T?m' = 'Tüm'
        'haklar?' = 'haklar?'
        'sakl?d?r' = 'sakl?d?r'
        'Dok?mantasyon' = 'Dokümantasyon'
        '??erik' = '?çerik'
        'Y?netimi' = 'Yönetimi'
        'T?m Sayfalar' = 'Tüm Sayfalar'
        '??retmen' = 'Ö?retmen'
        'Kullan?c?lar' = 'Kullan?c?lar'
        'Font Ayarlar?' = 'Font Ayarlar?'
        '?zel' = 'Özel'
        'YEN?' = 'YEN?'
        'Y?netici' = 'Yönetici'
        'Ba?l?k' = 'Ba?l?k'
        'Sat?r' = 'Sat?r'
        'Y?ksekli?i' = 'Yüksekli?i'
        'Harf Aral???' = 'Harf Aral???'
        'Geni?' = 'Geni?'
        '?ok Geni?' = 'Çok Geni?'
        'Varsay?lana S?f?rla' = 'Varsay?lana S?f?rla'
        '?nizle' = 'Önizle'
        'T?rk?e' = 'Türkçe'
        'karakterler i?in' = 'karakterler için'
        '?nerilir' = 'önerilir'
        'g?r?n?m' = 'görünüm'
        'tasar?m' = 'tasar?m'
        'aras?' = 'aras?'
        'i?in' = 'için'
        '?rne?i' = 'örne?i'
        'y?klenecek' = 'yüklenecek'
        'Olu?turma' = 'Olu?turma'
        'G?ncellenme' = 'Güncellenme'
    }

    foreach ($k in $map.Keys) {
        $text = $text.Replace($k, $map[$k])
    }

    return $text
}

$targets = @(
    'CoreBuilder.Admin\Shared\NavMenu.razor',
    'CoreBuilder.Admin\Shared\MainLayout.razor',
    'CoreBuilder.Admin\Pages\PageContentManager.razor',
    'CoreBuilder.Admin\Pages\AdminFontSettings.razor',
    'CoreBuilder\Models\FontSettings.cs'
)

foreach ($file in $targets) {
    if (-not (Test-Path $file)) { continue }
    $raw = Get-Content $file -Raw
    $fixed = Fix-Text $raw
    [System.IO.File]::WriteAllText($file, $fixed, [System.Text.Encoding]::UTF8)
    Write-Host "Fixed -> $file" -ForegroundColor Green
}

Write-Host "\nScan remaining mojibake markers..." -ForegroundColor Cyan
$remaining = Get-ChildItem -Recurse CoreBuilder.Admin -Include *.razor,*.cs |
    Select-String -Pattern '?|\?\w' -SimpleMatch -ErrorAction SilentlyContinue |
    Select-Object -First 200 |
    ForEach-Object { "$($_.Path):$($_.LineNumber):$($_.Line.Trim())" }

if ($remaining) {
    Write-Host "\nRemaining suspicious lines (please review):" -ForegroundColor Yellow
    $remaining | ForEach-Object { Write-Host $_ }
} else {
    Write-Host "No suspicious lines found." -ForegroundColor Green
}
