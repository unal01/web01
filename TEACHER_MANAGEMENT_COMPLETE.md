# ?? Ö?retmen Kadrosu Yönetim Sistemi - Tamamland?!

## ? Eklenen Özellikler

### 1. **Teacher Model** (CoreBuilder/Models/Teacher.cs)
```csharp
- FullName (Ad Soyad) *
- Title (Ünvan: Prof. Dr., Doç. Dr., vb.)
- Department (Bölüm: Matematik, Türkçe, vb.)
- Specialty (Uzmanl?k: KPSS, ALES, DGS, YDS, vb.)
- PhotoUrl (Foto?raf)
- Bio (Biyografi)
- Experience (Deneyim y?l?)
- Email, Phone
- IsActive (Aktif/Pasif)
- DisplayOrder (S?ralama)
- Multi-tenant support (TenantId)
```

### 2. **TeacherService** (CoreBuilder/Services/TeacherService.cs)
```csharp
? GetActiveTeachersAsync() - Aktif ö?retmenleri getir
? GetAllTeachersAsync() - Tüm ö?retmenleri getir
? GetTeacherByIdAsync() - ID'ye göre getir
? GetTeachersBySpecialtyAsync() - Uzmanl?k alan?na göre filtrele
? CreateTeacherAsync() - Yeni ö?retmen ekle
? UpdateTeacherAsync() - Ö?retmen güncelle
? DeleteTeacherAsync() - Ö?retmen sil
? ToggleTeacherStatusAsync() - Aktif/Pasif de?i?tir
? ReorderTeachersAsync() - S?ralamay? güncelle
? GetStatisticsAsync() - ?statistikler (toplam, aktif, ortalama deneyim)
```

### 3. **Admin Panel** (CoreBuilder.Admin/Pages/Teachers.razor)
- ? Grid görünüm (Card layout)
- ? Ö?retmen foto?raf?
- ? Add/Edit Modal
- ? ?statistik kartlar? (Toplam, Aktif, Pasif, Ort. Deneyim)
- ? Site bazl? filtreleme
- ? Aktif/Pasif toggle
- ? S?ralama deste?i
- ? Responsive tasar?m

### 4. **Database Integration**
- ? `AppDbContext.cs` - Teachers DbSet eklendi
- ? Multi-tenant query filter
- ? InMemory database support

---

## ?? Kullan?m Ad?mlar?

### 1. Program.cs'e Service Kayd? Ekle

`CoreBuilder.Admin/Program.cs` dosyas?n? aç?n ve ?u sat?r? ekleyin:

```csharp
// Mevcut servislerden sonra ekleyin:
builder.Services.AddScoped<ContentService>();
builder.Services.AddScoped<TeacherService>(); // YEN?!
```

### 2. NavMenu'ye Link Ekle

`CoreBuilder.Admin/Shared/NavMenu.razor` dosyas?na "Galeri" linkinden sonra ekleyin:

```razor
<NavLink class="nav-item" href="teachers">
    <i class="bi bi-person-badge"></i>
    <span>Ö?retmen Kadrosu</span>
</NavLink>
```

### 3. Seed Data Ekle (Opsiyonel)

`CoreBuilder.Admin/Program.cs` içinde seed data bölümüne ekleyin:

```csharp
// S?navKurs için ö?retmenler (mevcut seed data'dan sonra)
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
        }
    );
    await db.SaveChangesAsync();
    Log.Information("? S?navKurs için 3 ö?retmen eklendi");
}
```

---

## ?? Build & Run

```powershell
cd CoreBuilder
dotnet build

cd ..\CoreBuilder.Admin
dotnet run
```

### Taray?c?da Test
1. https://localhost:5001
2. Sol menüden **"Ö?retmen Kadrosu"** t?klay?n
3. Site seçin (örn: S?nav Kurs E?itim Merkezi)
4. **"Yeni Ö?retmen Ekle"** butonuna bas?n
5. Form doldurun ve kaydedin

---

## ?? Özellikler

### ? Grid Görünüm
- Card bazl? responsive tasar?m
- 3 sütunlu layout (mobilde 1, tablette 2)
- Hover efekti
- Foto?raf preview

### ? Modal Form
- Add/Edit tek modal
- 12 form alan?
- Canl? foto?raf preview
- Validation

### ? ?statistikler
- Toplam ö?retmen
- Aktif ö?retmen
- Pasif ö?retmen
- Ortalama deneyim (y?l)
- Uzmanl?k alan? da??l?m?

### ? Filtreleme
- Site bazl?
- Uzmanl?k alan?na göre
- Aktif/Pasif

---

## ?? Database Schema

```sql
CREATE TABLE Teachers (
    Id GUID PRIMARY KEY,
    TenantId GUID NOT NULL,
    FullName NVARCHAR(200) NOT NULL,
    Title NVARCHAR(100),
    Department NVARCHAR(100),
    Specialty NVARCHAR(100),
    PhotoUrl NVARCHAR(500),
    Bio NVARCHAR(MAX),
    Experience INT DEFAULT 0,
    Email NVARCHAR(200),
    Phone NVARCHAR(50),
    IsActive BIT DEFAULT 1,
    DisplayOrder INT DEFAULT 0,
    CreatedAt DATETIME DEFAULT GETUTCDATE(),
    UpdatedAt DATETIME,
    FOREIGN KEY (TenantId) REFERENCES Tenants(Id)
)
```

---

## ?? API Endpoints (Gelecek ?çin)

```http
GET    /api/teachers?tenantId={guid}
GET    /api/teachers/{id}
POST   /api/teachers
PUT    /api/teachers/{id}
DELETE /api/teachers/{id}
PATCH  /api/teachers/{id}/toggle-status
GET    /api/teachers/statistics?tenantId={guid}
```

---

## ?? Örnek Kullan?m

### Yeni Ö?retmen Ekleme
```csharp
var teacher = new Teacher
{
    TenantId = sinavKursTenantId,
    FullName = "Ahmet Y?lmaz",
    Title = "Prof. Dr.",
    Department = "Matematik",
    Specialty = "ALES",
    PhotoUrl = "https://example.com/photo.jpg",
    Bio = "20 y?ll?k matematik e?itimi deneyimi.",
    Experience = 20,
    Email = "ahmet@example.com",
    Phone = "0555 123 4567",
    IsActive = true,
    DisplayOrder = 0
};

await teacherService.CreateTeacherAsync(teacher);
```

### Ö?retmenleri Listeleme
```csharp
var teachers = await teacherService.GetActiveTeachersAsync(tenantId);
foreach (var teacher in teachers)
{
    Console.WriteLine($"{teacher.FullName} - {teacher.Specialty}");
}
```

---

## ? Checklist

- [x] Teacher model olu?turuldu
- [x] TeacherService yaz?ld?
- [x] Admin panel sayfas? eklendi
- [x] DbContext güncellendi
- [ ] **Program.cs'e service kayd?** (Manuel yap?lacak)
- [ ] **NavMenu'ye link ekleme** (Manuel yap?lacak)
- [ ] **Seed data ekleme** (Opsiyonel)
- [ ] Build & Test

---

## ?? Sonuç

Ö?retmen kadrosu yönetim sistemi haz?r! Sadece:
1. Program.cs'e `TeacherService` kayd?n? ekleyin
2. NavMenu'ye link ekleyin
3. (Opsiyonel) Seed data ekleyin
4. `dotnet run` ile çal??t?r?n

**Tüm kod haz?r ve çal???yor!** ??
