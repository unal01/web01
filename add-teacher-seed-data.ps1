# Add Teacher Seed Data to Program.cs
Write-Host "Adding Teacher Seed Data..." -ForegroundColor Yellow

$programFile = "CoreBuilder.Admin\Program.cs"
$content = Get-Content $programFile -Raw -Encoding UTF8

# Teacher seed data'y? ekle
$teacherSeedData = @'

        // 12. SINAVKURS TEACHERS
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

# S?navKurs duyurular?ndan sonra ekle
if ($content -notmatch "db\.Teachers\.Any")
{
    $content = $content -replace `
        '(Log\.Information\("? S?navKurs için 6 duyuru eklendi"\);)', `
        "`$1$teacherSeedData"
    
    [System.IO.File]::WriteAllText($programFile, $content, [System.Text.Encoding]::UTF8)
    Write-Host "? Teacher seed data eklendi!" -ForegroundColor Green
}
else
{
    Write-Host "??  Teacher seed data zaten var" -ForegroundColor Gray
}

Write-Host ""
Write-Host "Uygulamay? yeniden ba?lat?n:" -ForegroundColor Yellow
Write-Host "  cd CoreBuilder.Admin" -ForegroundColor Cyan
Write-Host "  dotnet run" -ForegroundColor Cyan
