# ?? Türkçe Karakter Sorunu Çözümü

## ?? Problem

Sayfalarda Türkçe karakterler (?, ?, ü, ö, ç, ?) düzgün görünmüyor veya bozuk karakterler gösteriliyor.

---

## ? Yap?lan Düzeltmeler

### 1. **_Host.cshtml** - UTF-8 Encoding

#### De?i?iklikler:
```html
<!-- Öncesi -->
<html lang="en">
<head>
    <meta charset="utf-8" />

<!-- Sonras? -->
<html lang="tr">
<head>
    <meta charset="utf-8" />
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
```

**Aç?klama:**
- `lang="tr"` ? Taray?c?ya Türkçe içerik oldu?unu bildirir
- `http-equiv="Content-Type"` ? HTTP header'?nda charset garanti eder

---

### 2. **ExamPrepSiteFactory.cs** - Verbatim String

#### Çift T?rnak Sorunu Düzeltildi:
```csharp
// YANLI? ?
<p class='fst-italic'>\"Ba?ar? hikayesi\"</p>

// DO?RU ?
<p class='fst-italic'>""Ba?ar? hikayesi""</p>
```

**Kural:** C# verbatim string'lerinde (`@"..."`) çift t?rnak için `""` kullan?l?r.

---

## ?? Olas? Türkçe Karakter Sorunlar?

### **1. Veritaban? Encoding**
```csharp
// InMemory Database kullan?yoruz - encoding sorunu olmamal?
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseInMemoryDatabase("CoreBuilderDb"));
```

? **InMemory DB** UTF-8'i native destekler.

### **2. Dosya Encoding**
Tüm `.cs` dosyalar? **UTF-8 with BOM** olarak kaydedilmeli.

**Visual Studio'da Kontrol:**
```
Dosya ? Geli?mi? Kaydetme Seçenekleri
Encoding: Unicode (UTF-8 with signature) - Codepage 65001
```

### **3. Blazor Render**
```razor
<!-- PublicPage.razor -->
@((MarkupString)currentPage.Content)
```

? `MarkupString` HTML'i düzgün render eder, encoding sorunu yaratmaz.

---

## ?? Test Senaryolar?

### **Test 1: Sayfa ?çeri?inde Türkçe**
```
1. Admin Panel ? Sayfalar ? S?navKurs seç
2. "Anasayfa" sayfas?n? düzenle
3. ?çeri?e bak?n:
   ? "S?nava Haz?rl?k Merkezi"
   ? "Ba?ar?l? Ö?renci"
   ? "Türkiye'nin en kaliteli..."
4. Taray?c?da önizleyin: /preview/{guid}/home
5. Türkçe karakterler düzgün mü?
```

### **Test 2: Slider'da Türkçe**
```
1. Admin Panel ? Slider ? S?navKurs seç
2. Bir slider'a bak:
   ? "Türkiye Yüksekö?retim Sistemi"
   ? "Üniversite Rehberi"
3. Site'de görüntüle
4. Türkçe karakterler bozuk mu?
```

### **Test 3: Duyurularda Türkçe**
```
1. Admin Panel ? Duyurular ? S?navKurs seç
2. Duyurular? kontrol et:
   ? "Ba?vurular?"
   ? "Ücretsiz Demo"
3. Türkçe sorun var m??
```

---

## ?? Manuel Düzeltme (Gerekirse)

### **Sayfa Düzenleme:**
```
1. Admin Panel ? Sayfalar
2. Sorunu olan sayfay? düzenle
3. ?çeri?i kopyala
4. Not Defteri'ne yap??t?r
5. Not Defteri ? Farkl? Kaydet ? UTF-8
6. Tekrar kopyala ve panele yap??t?r
7. Kaydet
```

### **Veritaban? Temizleme:**
```csharp
// Program.cs'de S?navKurs tenant'?n? sil ve tekrar olu?tur

// 1. Seed data'y? yorum sat?r?na al
/*
if (!db.Tenants.Any(t => t.Domain == "sinavkurs.localhost"))
{
    // ...
}
*/

// 2. Uygulamay? çal??t?r (InMemory DB temizlenir)

// 3. Yorum sat?r?n? kald?r ve tekrar çal??t?r
```

---

## ?? Browser Developer Tools

### **Encoding Kontrolü:**
```javascript
// Console'da çal??t?r
document.characterSet // "UTF-8" olmal?
```

### **Response Header Kontrolü:**
```
Network Tab ? Herhangi bir sayfa iste?i
? Response Headers:
   Content-Type: text/html; charset=utf-8
```

---

## ?? Türkçe Karakter Test Listesi

### **Büyük Harfler:**
```
? ? ? Ü Ö Ç ?
```

### **Küçük Harfler:**
```
? ? ? ü ö ç ?
```

### **Özel Karakterler:**
```
? ? (Türk Liras?)
? " " (T?rnak i?aretleri)
? … (Üç nokta)
```

---

## ?? Çözüm Ad?mlar? (Özet)

### **1. HTML Meta Tag:**
```html
<meta charset="utf-8" />
<meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
```

### **2. HTML Lang:**
```html
<html lang="tr">
```

### **3. Dosya Encoding:**
```
Tüm .cs dosyalar?: UTF-8 with BOM
```

### **4. Verbatim String:**
```csharp
// Çift t?rnak için "" kullan
@"?çinde ""t?rnak"" var"
```

### **5. Browser Cache:**
```
Ctrl + F5 (Hard Refresh)
veya
Taray?c? geli?tirici araçlar?nda "Disable Cache"
```

---

## ?? Charset Hierarchy

```
1. HTTP Response Header
   ?
2. HTML Meta Tag (http-equiv)
   ?
3. HTML Meta Tag (charset)
   ?
4. Browser Default (UTF-8)
```

**Öncelik:** En yukar?daki geçerlidir.

---

## ?? Sonuç

### **Düzeltmeler:**
- ? `_Host.cshtml` ? `lang="tr"` + `Content-Type` meta
- ? `ExamPrepSiteFactory.cs` ? Verbatim string çift t?rnak
- ? Dosya encoding kontrolü

### **Beklenen Davran??:**
- ? Tüm sayfalar Türkçe karakter destekli
- ? Slider'lar ve duyurular düzgün görünüyor
- ? Admin panel'de düzenleme sorunsuz

### **Test:**
```powershell
cd C:\Users\User\source\repos\unal01\web01\CoreBuilder.Admin
dotnet run
```

```
?? http://sinavkurs.localhost:5001
? Türkçe karakterler düzgün mü?
```

---

**Durum:** ? Düzeltildi  
**Test:** Gerekli  
**Notlar:** InMemory DB ve UTF-8 BOM dosya encoding'i garanti eder
