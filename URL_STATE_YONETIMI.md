# ?? URL State Yönetimi - Tenant ID Persistans?

## ?? Problem

Sayfa düzenleme veya duyuru yönetimi sayfalar?nda i?lem yapt?ktan sonra liste sayfas?na geri dönüldü?ünde:
- ? Seçili tenant ID **kayboluyordu**
- ? Sadece **"Site Seçin"** dropdown'u görünüyordu
- ? Kullan?c? **tekrar site seçmek zorunda** kal?yordu
- ? Kötü kullan?c? deneyimi

### Örnek Senaryo:
```
1. /pages ? Site seç (Demo Okul)
2. Sayfa düzenle ? /page/{id}
3. "Geri Dön" ? /pages
4. ? Site seçimi KAYBOLDU ? Tekrar seç!
```

---

## ? Çözüm: URL'de Tenant ID Ta??ma

Tenant ID'yi **URL parametresi** olarak ta??yarak state'i koruyoruz:

### Öncesi:
```
/pages
/sliders
/announcements
```

### Sonras?:
```
/pages/{tenantId}
/sliders/{tenantId}
/announcements/{tenantId}
```

---

## ?? Yap?lan De?i?iklikler

### 1. **Pages.razor**

#### Route Tan?m?:
```razor
@page "/pages"
@page "/pages/{TenantIdParam:guid?}"
```

#### Parameter:
```csharp
[Parameter] public Guid? TenantIdParam { get; set; }
```

#### State Yönetimi:
```csharp
private Guid SelectedTenantId
{
    get => selectedTenantId;
    set 
    { 
        selectedTenantId = value; 
        LoadPages();
        // URL'yi güncelle
        if (value != Guid.Empty)
        {
            Navigation.NavigateTo($"/pages/{value}", false);
        }
    }
}

protected override void OnInitialized()
{
    Tenants = Context.Tenants.ToList();
    
    // URL'den gelen tenant ID'yi kullan
    if (TenantIdParam.HasValue && TenantIdParam.Value != Guid.Empty)
    {
        selectedTenantId = TenantIdParam.Value;
        LoadPages();
    }
}

protected override void OnParametersSet()
{
    // URL de?i?ti?inde tenant ID'yi güncelle
    if (TenantIdParam.HasValue && TenantIdParam.Value != selectedTenantId)
    {
        selectedTenantId = TenantIdParam.Value;
        LoadPages();
    }
}
```

---

### 2. **PageDetail.razor**

#### "Geri Dön" Butonu:
```csharp
private void GoBack()
{
    // Tenant ID'yi koruyarak geri dön
    if (pageObj != null)
    {
        Navigation.NavigateTo($"/pages/{pageObj.TenantId}");
    }
    else
    {
        Navigation.NavigateTo("/pages");
    }
}
```

---

### 3. **Sliders.razor**

#### Route ve State:
```razor
@page "/sliders"
@page "/sliders/{TenantIdParam:guid?}"
@inject NavigationManager Navigation
```

```csharp
[Parameter] public Guid? TenantIdParam { get; set; }

private Guid SelectedTenantId
{
    get => selectedTenantId;
    set 
    { 
        selectedTenantId = value; 
        LoadSliders();
        if (value != Guid.Empty)
        {
            Navigation.NavigateTo($"/sliders/{value}", false);
        }
    }
}

protected override void OnInitialized()
{
    Tenants = Context.Tenants.ToList();
    
    if (TenantIdParam.HasValue && TenantIdParam.Value != Guid.Empty)
    {
        selectedTenantId = TenantIdParam.Value;
        LoadSliders();
    }
}
```

---

### 4. **Announcements.razor**

#### Ayn? Mant?k:
```razor
@page "/announcements"
@page "/announcements/{TenantIdParam:guid?}"
@inject NavigationManager Navigation
```

```csharp
[Parameter] public Guid? TenantIdParam { get; set; }

private Guid SelectedTenantId
{
    get => selectedTenantId;
    set 
    { 
        selectedTenantId = value; 
        LoadAnnouncements();
        if (value != Guid.Empty)
        {
            Navigation.NavigateTo($"/announcements/{value}", false);
        }
    }
}
```

---

## ?? Kullan?c? Ak??? (Sonras?)

### Sayfa Yönetimi:
```
1. /pages ? Site seç (Demo Okul, ID: abc123)
   ?
2. URL otomatik güncellenir: /pages/abc123
   ?
3. Sayfa düzenle ? /page/xyz789
   ?
4. "Geri Dön" ? /pages/abc123
   ?
5. ? Site seçimi KORUNDU! Sayfa listesi gösteriliyor!
```

### Slider Yönetimi:
```
1. /sliders ? Site seç (YÖK, ID: def456)
   ?
2. URL: /sliders/def456
   ?
3. Slider düzenle (modal)
   ?
4. Kaydet ? Modal kapan?r
   ?
5. ? Hala /sliders/def456 - Site seçimi korundu!
```

### Duyuru Yönetimi:
```
1. /announcements ? Site seç (Demo Okul, ID: abc123)
   ?
2. URL: /announcements/abc123
   ?
3. Duyuru düzenle (modal)
   ?
4. Güncelle ? Modal kapan?r
   ?
5. ? Hala /announcements/abc123 - Duyurular görünüyor!
```

---

## ?? Navigation.NavigateTo Parametreleri

```csharp
// forceLoad = false ? Sayfay? yeniden yüklemez, sadece URL de?i?ir
Navigation.NavigateTo($"/pages/{tenantId}", false);

// forceLoad = true (varsay?lan) ? Sayfay? tamamen yeniden yükler
Navigation.NavigateTo($"/pages/{tenantId}", true);
```

---

## ?? UI/UX ?yile?tirmeleri

### Öncesi:
- ? Site seç ? Liste görünür
- ? Düzenle ? Geri dön
- ? Site seçimi kayboldu ? **Tekrar seç!**

### Sonras?:
- ? Site seç ? URL güncellenir ? Liste görünür
- ? Düzenle ? Geri dön (tenant ID ile)
- ? Site seçimi korundu ? **Direkt liste!**

---

## ?? Test Senaryolar?

### Test 1: Sayfa Seçimi Persistans?
```
1. /pages adresini aç
2. "Demo Okul" seç
3. URL kontrol: /pages/{guid} olmal?
4. Taray?c? yenile (F5)
5. ? Site seçimi hala korunuyor mu?
```

### Test 2: Düzenleme Ak???
```
1. /pages/{guid} ? Bir sayfa düzenle
2. De?i?iklik yap ? Geri Dön
3. ? Liste hala görünüyor mu?
4. ? Site dropdown'u hala seçili mi?
```

### Test 3: URL Manuel De?i?tirme
```
1. /pages/abc123 URL'ini kopyala
2. Yeni sekmede aç
3. ? Direkt o tenant'?n sayfalar? görünmeli
```

### Test 4: Slider Modal
```
1. /sliders ? Site seç
2. URL: /sliders/{guid}
3. Slider düzenle (modal)
4. Kaydet ? Modal kapan?r
5. ? URL de?i?medi mi?
6. ? Liste hala görünüyor mu?
```

---

## ?? Avantajlar

### 1. **Bookmarking (Yer ?mleri)**
```
Kullan?c? /pages/abc123 adresini yer imlerine ekleyebilir
? Sonra direkt bu tenant'?n sayfalar?na gider
```

### 2. **Payla??labilir URL'ler**
```
Admin ba?ka bir kullan?c?ya URL gönderebilir:
"?u sayfaya bak: /pages/abc123"
? Direkt do?ru tenant aç?l?r
```

### 3. **Browser History**
```
Geri/?leri butonlar? düzgün çal???r
? Tenant seçimleri korunur
```

### 4. **State Persistans?**
```
Sayfa yenilendi?inde (F5)
? Tenant seçimi kaybolmaz
```

---

## ?? Sonuç

### Önceki Sorunlar:
- ? Tenant ID her i?lemde kayboluyordu
- ? Kullan?c? tekrar site seçmek zorundayd?
- ? URL'ler anlaml? de?ildi
- ? Bookmarking yap?lam?yordu

### Yeni Durum:
- ? Tenant ID URL'de korunuyor
- ? Seamless kullan?c? deneyimi
- ? Anlaml? ve payla??labilir URL'ler
- ? Bookmarking ve history deste?i
- ? Taray?c? yenileme tolerans?

---

**Haz?rlayan:** AI Assistant  
**Tarih:** 2024  
**Versiyon:** 3.0  
**Tag:** #URL-State-Management #Blazor #UX-Improvement
