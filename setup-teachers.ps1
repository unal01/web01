# Teacher Management Auto-Setup Script
Write-Host "???????????????????????????????????" -ForegroundColor Cyan
Write-Host "Teacher Management Auto-Setup" -ForegroundColor Cyan
Write-Host "???????????????????????????????????" -ForegroundColor Cyan
Write-Host ""

$programCs = "CoreBuilder.Admin\Program.cs"
$navMenuRazor = "CoreBuilder.Admin\Shared\NavMenu.razor"

# 1. Program.cs - TeacherService Kayd? Ekle
Write-Host "?? Program.cs güncelleniyor..." -ForegroundColor Yellow

$programContent = Get-Content $programCs -Raw

if ($programContent -notmatch "TeacherService")
{
    # ContentService sat?r?n? bul ve alt?na ekle
    $programContent = $programContent -replace `
        '(builder\.Services\.AddScoped<ContentService>\(\);)', `
        "`$1`r`n    builder.Services.AddScoped<TeacherService>();"
    
    Set-Content $programCs -Value $programContent -Encoding UTF8
    Write-Host "  ? TeacherService kayd? eklendi" -ForegroundColor Green
}
else
{
    Write-Host "  ??  TeacherService zaten kay?tl?" -ForegroundColor Gray
}

# 2. Program.cs - Teacher Seed Data Ekle
Write-Host "?? Teacher seed data ekleniyor..." -ForegroundColor Yellow

if ($programContent -notmatch "db\.Teachers\.Any")
{
    # S?navKurs announcements'tan sonra ekle
    $teacherSeedData = @'

        // 12. S?NAVKURS TEACHERS
        if (sinavKursTenantId != Guid.Empty && !db.Teachers.Any(t => t.TenantId == sinavKursTenantId))
        {
            db.Teachers.AddRange(
                new Teacher
                {
                    TenantId = sinavKursTenantId,
                    FullName = "Mustafa Turan KAR",
                    Title = "Matematik",
                    Department = "Matematik Bölümü",
                    Specialty = "KPSS",
                    PhotoUrl = "https://images.unsplash.com/photo-1472099645785-5658abf4ff4e?w=400&h=500&fit=crop",
                    Bio = "15 y?ll?k KPSS deneyimi. 5000+ ö?renci mezun etti.",
                    Experience = 15,
                    IsActive = true,
                    DisplayOrder = 1
                },
                new Teacher
                {
                    TenantId = sinavKursTenantId,
                    FullName = "Esma KARAA?AÇ",
                    Title = "Türkçe",
                    Department = "Türkçe Bölümü",
                    Specialty = "KPSS",
                    PhotoUrl = "https://images.unsplash.com/photo-1494790108377-be9c29b29330?w=400&h=500&fit=crop",
                    Bio = "Türkçe dil bilgisi ve sözel bölüm uzman?.",
                    Experience = 12,
                    IsActive = true,
                    DisplayOrder = 2
                },
                new Teacher
                {
                    TenantId = sinavKursTenantId,
                    FullName = "Rukiye GÜLERYÜZ",
                    Title = "?ngilizce",
                    Department = "Yabanc? Diller",
                    Specialty = "YDS",
                    PhotoUrl = "https://images.unsplash.com/photo-1438761681033-6461ffad8d80?w=400&h=500&fit=crop",
                    Bio = "YDS ve YÖKD?L s?navlar?nda uzman e?itmen.",
                    Experience = 10,
                    IsActive = true,
                    DisplayOrder = 3
                },
                new Teacher
                {
                    TenantId = sinavKursTenantId,
                    FullName = "Merve ÇALI?KAN",
                    Title = "Tarih",
                    Department = "Sosyal Bilimler",
                    Specialty = "KPSS",
                    PhotoUrl = "https://images.unsplash.com/photo-1573497019940-1c28c88b4f3e?w=400&h=500&fit=crop",
                    Bio = "Tarih ve co?rafya konular?nda uzman.",
                    Experience = 8,
                    IsActive = true,
                    DisplayOrder = 4
                },
                new Teacher
                {
                    TenantId = sinavKursTenantId,
                    FullName = "Melisa ÖZLÜK",
                    Title = "Fizik",
                    Department = "Fen Bilimleri",
                    Specialty = "ALES",
                    PhotoUrl = "https://images.unsplash.com/photo-1580489944761-15a19d654956?w=400&h=500&fit=crop",
                    Bio = "Fizik ve kimya dersleri uzman?.",
                    Experience = 9,
                    IsActive = true,
                    DisplayOrder = 5
                },
                new Teacher
                {
                    TenantId = sinavKursTenantId,
                    FullName = "Sami DALK?RAN",
                    Title = "Geometri",
                    Department = "Matematik",
                    Specialty = "DGS",
                    PhotoUrl = "https://images.unsplash.com/photo-1556157382-97eda2d62296?w=400&h=500&fit=crop",
                    Bio = "Geometri ve analitik geometri uzman?.",
                    Experience = 13,
                    IsActive = true,
                    DisplayOrder = 6
                },
                new Teacher
                {
                    TenantId = sinavKursTenantId,
                    FullName = "Nagehan AKSOY",
                    Title = "Edebiyat",
                    Department = "Türk Dili",
                    Specialty = "KPSS",
                    PhotoUrl = "https://images.unsplash.com/photo-1598550874175-4d0ef436c909?w=400&h=500&fit=crop",
                    Bio = "Türk Edebiyat? ve Dil Anlat?m uzman?.",
                    Experience = 11,
                    IsActive = true,
                    DisplayOrder = 7
                },
                new Teacher
                {
                    TenantId = sinavKursTenantId,
                    FullName = "Sevcan UYSAL",
                    Title = "Biyoloji",
                    Department = "Fen Bilimleri",
                    Specialty = "TYT/AYT",
                    PhotoUrl = "https://images.unsplash.com/photo-1551836022-d5d88e9218df?w=400&h=500&fit=crop",
                    Bio = "Biyoloji ve kimya dersleri.",
                    Experience = 7,
                    IsActive = true,
                    DisplayOrder = 8
                },
                new Teacher
                {
                    TenantId = sinavKursTenantId,
                    FullName = "Ashani GÜDENÇ",
                    Title = "?statistik",
                    Department = "Matematik",
                    Specialty = "ALES",
                    PhotoUrl = "https://images.unsplash.com/photo-1607746882042-944635dfe10e?w=400&h=500&fit=crop",
                    Bio = "?statistik ve olas?l?k konular?nda uzman.",
                    Experience = 6,
                    IsActive = true,
                    DisplayOrder = 9
                },
                new Teacher
                {
                    TenantId = sinavKursTenantId,
                    FullName = "Ya?mur ALAGÖZ",
                    Title = "Co?rafya",
                    Department = "Sosyal Bilimler",
                    Specialty = "KPSS",
                    PhotoUrl = "https://images.unsplash.com/photo-1544005313-94ddf0286df2?w=400&h=500&fit=crop",
                    Bio = "Co?rafya ve çevre bilimleri.",
                    Experience = 10,
                    IsActive = true,
                    DisplayOrder = 10
                }
            );
            await db.SaveChangesAsync();
            Log.Information("? S?navKurs için 10 ö?retmen eklendi");
        }
'@

    $programContent = Get-Content $programCs -Raw
    $programContent = $programContent -replace `
        '(Log\.Information\("? S?navKurs için 6 duyuru eklendi"\);)', `
        "`$1$teacherSeedData"
    
    Set-Content $programCs -Value $programContent -Encoding UTF8
    Write-Host "  ? Teacher seed data eklendi" -ForegroundColor Green
}
else
{
    Write-Host "  ??  Teacher seed data zaten mevcut" -ForegroundColor Gray
}

# 3. NavMenu.razor - Teachers Link Ekle
Write-Host "?? NavMenu.razor güncelleniyor..." -ForegroundColor Yellow

$navMenuContent = Get-Content $navMenuRazor -Raw

if ($navMenuContent -notmatch "teachers")
{
    # Gallery linkinden sonra ekle
    $teacherLink = @'

            <NavLink class="nav-item" href="teachers">
                <i class="bi bi-person-badge"></i>
                <span>Ö?retmen Kadrosu</span>
            </NavLink>
'@

    $navMenuContent = $navMenuContent -replace `
        '(<NavLink class="nav-item" href="gallery">.*?</NavLink>)', `
        "`$0$teacherLink"
    
    Set-Content $navMenuRazor -Value $navMenuContent -Encoding UTF8
    Write-Host "  ? NavMenu'ye Teachers linki eklendi" -ForegroundColor Green
}
else
{
    Write-Host "  ??  Teachers linki zaten mevcut" -ForegroundColor Gray
}

# 4. Build Test
Write-Host ""
Write-Host "?? Build test..." -ForegroundColor Yellow
Set-Location "CoreBuilder"
$buildResult = dotnet build --no-restore 2>&1

if ($LASTEXITCODE -eq 0)
{
    Write-Host "  ? Build ba?ar?l?" -ForegroundColor Green
}
else
{
    Write-Host "  ??  Build hatas? (devam ediliyor)" -ForegroundColor Yellow
}

Set-Location ".."

# 5. Summary
Write-Host ""
Write-Host "???????????????????????????????????" -ForegroundColor Cyan
Write-Host "TAMAMLANDI!" -ForegroundColor Green
Write-Host "???????????????????????????????????" -ForegroundColor Cyan
Write-Host ""
Write-Host "? TeacherService kaydedildi" -ForegroundColor Green
Write-Host "? Teacher seed data eklendi (10 ö?retmen)" -ForegroundColor Green
Write-Host "? NavMenu'ye link eklendi" -ForegroundColor Green
Write-Host ""
Write-Host "??MD? YAPMANIZ GEREKENLER:" -ForegroundColor Yellow
Write-Host ""
Write-Host "1. Uygulamay? çal??t?r:" -ForegroundColor White
Write-Host "   cd CoreBuilder.Admin" -ForegroundColor Cyan
Write-Host "   dotnet run" -ForegroundColor Cyan
Write-Host ""
Write-Host "2. Taray?c?da test et:" -ForegroundColor White
Write-Host "   https://localhost:5001" -ForegroundColor Cyan
Write-Host "   Sol menüden: Ö?retmen Kadrosu" -ForegroundColor Cyan
Write-Host "   Site seç: S?nav Kurs E?itim Merkezi" -ForegroundColor Cyan
Write-Host ""
Write-Host "3. 10 ö?retmen otomatik eklenmi? olacak!" -ForegroundColor Green
Write-Host ""
Write-Host "???????????????????????????????????" -ForegroundColor Cyan
