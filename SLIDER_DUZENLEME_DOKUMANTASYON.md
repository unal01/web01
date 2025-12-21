# ??? Slider Yönetimi - Düzenleme Özelli?i Eklendi

## ? Yap?lan De?i?iklikler

### 1. **Sliders.razor** - Düzenleme UI Eklendi
- ?? Her slider sat?r?na **"Düzenle" butonu** eklendi
- ?? Modal ba?l??? dinamik hale getirildi: "Yeni Slide Ekle" / "Slide Düzenle"
- ?? Kaydet butonu metni dinamik: "Kaydet" / "Güncelle"
- ?? `OpenEditModal(Slider slider)` metodu eklendi - Slider verilerini forma doldurur
- ?? `editingSliderId` de?i?keni eklendi - Düzenleme modunu takip eder

### 2. **ContentService.cs** - Yeni Metodlar
- ? `GetSliderByIdAsync(Guid id)` - ID'ye göre slider getir
- ? `UpdateSliderAsync(Slider slider)` - Mevcut slider'? güncelle
- ? `GetAllSlidersAsync(Guid tenantId)` - Tüm sliderlar? getir (aktif + pasif)

### 3. **Düzenleme Ak???**
```
1. Kullan?c? "Düzenle" butonuna t?klar
2. OpenEditModal() slider verilerini forma doldurur
3. editingSliderId set edilir (Guid.Empty de?il)
4. Modal aç?l?r (ba?l?k: "Slide Düzenle")
5. Kullan?c? de?i?iklikleri yapar
6. "Güncelle" butonuna t?klar
7. SaveSlider() metodu çal???r:
   - editingSliderId == Guid.Empty ise ? YEN? EKLE
   - editingSliderId != Guid.Empty ise ? GÜNCELLE
8. ContentService.UpdateSliderAsync() ça?r?l?r
9. Modal kapan?r ve liste yenilenir
```

## ?? Kullan?m

### Slider Düzenleme Ad?mlar?:
1. **Admin Panel**'de `/sliders` sayfas?na git
2. Site seç (dropdown)
3. Düzenlemek istedi?in slider'?n yan?ndaki **?? Kalem** butonuna t?kla
4. Formdaki bilgileri düzenle:
   - Ba?l?k
   - Aç?klama
   - Resim URL
   - Buton Metni
   - Buton Link
   - S?ra
   - Aktif/Pasif durumu
5. **"Güncelle"** butonuna t?kla
6. ? Ba?ar? mesaj? görünür: "Slide ba?ar?yla güncellendi!"

### Özellikler:
- ?? **Tüm sliderlar listelenir** (aktif + pasif)
- ?? **Düzenleme** - Kalem butonuyla
- ? **Yeni Ekleme** - "Yeni Slide Ekle" butonu
- ??? **Silme** - Çöp kutusu butonu
- ??? **Durum göstergesi** - Ye?il (Aktif) / Gri (Pasif) badge
- ?? **S?ralama** - Order de?eri mavi badge
- ??? **Thumbnail** - Slider görseli küçük önizleme

## ?? Test Etme

```powershell
cd C:\Users\User\source\repos\unal01\web01\CoreBuilder.Admin
dotnet run
```

Taray?c?da:
1. https://localhost:5001 aç
2. Login: `admin` / `Admin123!`
3. Sol menüden **"Slider Yönetimi"** seç
4. Site seç: **Demo Okul** veya **YÖK**
5. Herhangi bir slider'? düzenle
6. Ba?ar?yla güncellendi?ini gör! ?

## ?? API Endpoints

ContentService metodlar?:
```csharp
// Sadece aktif sliderlar (frontend için)
GetSlidersAsync(Guid tenantId)

// Tüm sliderlar (admin panel için)
GetAllSlidersAsync(Guid tenantId)

// ID'ye göre getir
GetSliderByIdAsync(Guid id)

// Yeni ekle
CreateSliderAsync(Slider slider)

// Güncelle
UpdateSliderAsync(Slider slider)

// Sil
DeleteSliderAsync(Guid id)
```

## ?? UI De?i?iklikleri

**Öncesi:**
```
[Görsel] [Ba?l?k] [Aç?klama] [S?ra] [Durum] [??? Sil]
```

**Sonras?:**
```
[Görsel] [Ba?l?k] [Aç?klama] [S?ra] [Durum] [?? Düzenle] [??? Sil]
```

## ? Bonus ?yile?tirmeler

- ? Modal ba?l??? context-aware (Ekle/Düzenle)
- ? Kaydet butonu text dinamik (Kaydet/Güncelle)
- ? Pasif sliderlar da görünür
- ? Düzenleme s?ras?nda mevcut de?erler otomatik doldurulur
- ? Ba?ar? mesajlar? net ve anla??l?r

---

**Haz?rlayan:** AI Assistant  
**Tarih:** 2024  
**Versiyon:** 1.0
