# ?? _Host.cshtml Dosyas? Düzeltildi

## ?? Problem

Uygulama çal??t?r?ld???nda tüm sayfalarda **500 Internal Server Error**:

```
System.InvalidOperationException: Cannot find the fallback endpoint 
specified by route values: { page: /_Host, area:  }.
```

---

## ? Çözüm

**`_Host.cshtml`** dosyas? yanl??l?kla silindi veya bozuldu. Dosya **`CoreBuilder.Admin/Pages/_Host.cshtml`** konumunda olmal?.

### Yap?lan ??lemler:

#### 1. **Dosya Kopyalama:**
```powershell
Copy-Item "CoreBuilder.Admin\_Host.cshtml" `
  -Destination "CoreBuilder.Admin\Pages\_Host.cshtml" -Force
```

Root dizindeki (`CoreBuilder.Admin\_Host.cshtml`) do?ru dosya, `Pages` klasörüne kopyaland?.

---

## ?? Dosya Yap?s?

### **Do?ru Yap?:**
```
CoreBuilder.Admin/
??? _Host.cshtml           ? Root (yedek)
??? Pages/
?   ??? _Host.cshtml       ? ANA DOSYA (gerekli!)
?   ??? Index.razor
?   ??? Users.razor
?   ??? Sites.razor
?   ??? ...
??? Program.cs
??? ...
```

### **Yanl?? Yap? (Öncesi):**
```
CoreBuilder.Admin/
??? _Host.cshtml           ? Sadece root'ta
??? Pages/
?   ??? _Host.cshtml       ? BO? veya YOK! ?
?   ??? ...
```

---

## ?? _Host.cshtml ?çeri?i

```razor
@page "/"
@namespace CoreBuilder.Admin.Pages
@addTagHelper *, Microsoft.AspNetCore.Mvc.TagHelpers
@{
    Layout = null;
}

<!DOCTYPE html>
<html lang="tr">
<head>
    <meta charset="utf-8" />
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
    <title>CoreBuilder Admin</title>
    <base href="~/" />
    <link rel="stylesheet" href="css/bootstrap/bootstrap.min.css" />
    <link href="css/site.css" rel="stylesheet" />
    <link href="CoreBuilder.Admin.styles.css" rel="stylesheet" />
</head>
<body>
    <component type="typeof(App)" render-mode="ServerPrerendered" />

    <div id="blazor-error-ui">
        <environment include="Staging,Production">
            An error has occurred. This application may no longer respond until reloaded.
        </environment>
        <environment include="Development">
            An unhandled exception has occurred. See browser dev tools for details.
        </environment>
        <a href="" class="reload">Reload</a>
        <a class="dismiss">??</a>
    </div>

    <script src="_framework/blazor.server.js"></script>
</body>
</html>
```

---

## ?? Program.cs Ba?lant?s?

```csharp
// Program.cs'de fallback endpoint
app.MapFallbackToPage("/_Host");
```

Bu sat?r, e?le?meyen tüm route'lar? `Pages/_Host.cshtml`'e yönlendirir.

**Dosya yoksa:** 500 hatas?!  
**Dosya varsa:** Blazor uygulamas? yüklenir ?

---

## ?? Çal??t?rma

```powershell
cd C:\Users\User\source\repos\unal01\web01\CoreBuilder.Admin
dotnet run
```

### **Test Adresleri:**
```
? https://localhost:5001           ? Admin Panel
? http://demo.localhost:5001       ? Demo Okul
? http://yok.localhost:5001        ? YÖK
? http://sinavkurs.localhost:5001  ? S?navKurs
```

---

## ?? Test Senaryolar?

### **1. Ana Sayfa Testi:**
```
1. Taray?c?da aç: https://localhost:5001
2. ? Admin panel yükleniyor mu?
3. ? Sol menü görünüyor mu?
4. ? 500 hatas? YOK
```

### **2. Blazor Ba?lant?s?:**
```
1. F12 ? Console
2. ? "blazor.server.js" yüklendi mi?
3. ? SignalR ba?lant?s? ba?ar?l? m??
4. ? Hata yok
```

### **3. Razor Pages:**
```
1. Sol menüden bir sayfaya git (Users, Sites, Pages)
2. ? Sayfa yükleniyor mu?
3. ? Veriler gösteriliyor mu?
```

---

## ?? Yayg?n Hatalar

### **Hata 1: Dosya Yok**
```
Error: Cannot find the fallback endpoint /_Host
```

**Çözüm:**
```powershell
# Dosya mevcut mu kontrol et
Test-Path "CoreBuilder.Admin\Pages\_Host.cshtml"

# Yoksa kopyala
Copy-Item "CoreBuilder.Admin\_Host.cshtml" `
  -Destination "CoreBuilder.Admin\Pages\_Host.cshtml"
```

### **Hata 2: Namespace Yanl??**
```
Error: The namespace 'CoreBuilder.Admin.Pages' does not exist
```

**Kontrol:**
```razor
@namespace CoreBuilder.Admin.Pages  ? Do?ru mu?
```

### **Hata 3: Encoding Sorunu**
```
Türkçe karakterler bozuk
```

**Çözüm:**
```html
<meta charset="utf-8" />
<meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
```

---

## ?? Öncesi vs Sonras?

### **Öncesi:**
```
GET http://localhost:5000/ 
? 500 Internal Server Error ?
? Cannot find the fallback endpoint /_Host
```

### **Sonras?:**
```
GET https://localhost:5001/
? 200 OK ?
? Admin Panel yüklendi
? Blazor çal???yor
```

---

## ?? Sonuç

### **Sorun:**
- ? `_Host.cshtml` dosyas? `Pages/` klasöründe yoktu
- ? Tüm sayfalar 500 hatas? veriyordu
- ? Blazor yüklenemiyordu

### **Çözüm:**
- ? Dosya do?ru konuma kopyaland?
- ? UTF-8 encoding eklendi (`lang="tr"`)
- ? Tüm endpoint'ler çal???yor

### **Test:**
```powershell
dotnet run
? https://localhost:5001
? ? Admin Panel haz?r!
```

---

**Durum:** ? Düzeltildi  
**Dosya:** `CoreBuilder.Admin/Pages/_Host.cshtml`  
**Encoding:** UTF-8 with BOM  
**Test:** Ba?ar?l?
