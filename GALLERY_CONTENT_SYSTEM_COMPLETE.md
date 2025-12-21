# ?? GALER? ?ÇER?K YÖNET?M? - HAZIR!

## ? **Tamamlanan Özellikler**

### 1. **Backend** ?
- `PageContentService.cs` ? CRUD i?lemleri, istatistikler
- `PageContent` model ? Database'e kaydedildi
- AppDbContext güncellendi
- Build ba?ar?l? ?

### 2. **Frontend** ?
- `PageContentManager.razor` ? ?çerik yönetim ekran?
- Foto?raf yükleme
- Video embed (YouTube/Vimeo)
- Lightbox (resim büyütme)
- ?statistik kartlar?

### 3. **Entegrasyon** ?
- `PageDetail.razor` ? "?çerikleri Yönet" butonu eklendi
- `Program.cs` ? PageContentService kaydedildi

---

## ?? **Kullan?m Ad?mlar?**

### **Ad?m 1: Uygulamay? Ba?lat**
```powershell
cd CoreBuilder.Admin
dotnet run
```

### **Ad?m 2: Galeri Sayfas?na Git**
1. **https://localhost:5001/pages**
2. Site seç: **"S?nav Kurs E?itim Merkezi"**
3. **"galeri"** sayfas?na t?kla

### **Ad?m 3: ?çerik Ekle**
1. **"?? ?çerikleri Yönet"** butonuna t?kla
2. **"Yeni ?çerik Ekle"** t?kla
3. Tür seç: **??? Foto?raf** veya **?? Video**

#### **Foto?raf Yükleme:**
- Ba?l?k: "Kampüs Görünümü"
- Aç?klama: "Ana giri? kap?s?"
- **Dosya Seç** ? Foto?raf yükle
- **Kaydet**

#### **Video Ekleme:**
- Ba?l?k: "Tan?t?m Videosu"
- YouTube URL: `https://www.youtube.com/watch?v=dQw4w9WgXcQ`
- **Kaydet**

---

## ?? **Özellikler**

### **Foto?raf Yükleme**
```
???????????????????????????????
?  [Dosya Seç] Yükleniyor...  ?
?  ? Foto?raf yüklendi!       ?
?  [Önizleme Resmi]            ?
???????????????????????????????
```

### **Video Embed**
- YouTube URL gir
- Otomatik embed kodu olu?turulur
- Thumbnail otomatik al?n?r
- Önizleme gösterilir

### **Lightbox**
- Foto?rafa t?kla
- Tam ekran görüntüleme
- `×` ile kapat
- T?klayarak kapat

### **?statistikler**
```
???????????????????????????????????????????????????
?Toplam: 5? ??? 3     ? ?? 2     ?Aktif: 5 ?Pasif: 0 ?
???????????????????????????????????????????????????
```

---

## ?? **Grid Layout (Otomatik)**

Yükledi?iniz içerikler otomatik olarak grid layout'ta gösterilir:

```
???????????????  ???????????????  ???????????????  ???????????????
? [FOTO?RAF]  ?  ? [FOTO?RAF]  ?  ?  [VIDEO]    ?  ? [FOTO?RAF]  ?
? Kampüs Giri??  ? Kütüphane   ?  ? Tan?t?m     ?  ? Spor Salonu ?
? ??? #1 Aktif ?  ? ??? #2 Aktif ?  ? ?? #3 Aktif ?  ? ??? #4 Aktif ?
?[Düzenle][Sil]?  ?[Düzenle][Sil]?  ?[Düzenle][Sil]?  ?[Düzenle][Sil]?
???????????????  ???????????????  ???????????????  ???????????????
```

---

## ?? **Video Deste?i**

### **Desteklenen Platformlar:**
- ? YouTube
- ? Vimeo

### **YouTube Örnek:**
```
URL: https://www.youtube.com/watch?v=dQw4w9WgXcQ

Otomatik olu?turulur:
- Embed: <iframe src="https://youtube.com/embed/dQw4w9WgXcQ">
- Thumbnail: https://img.youtube.com/vi/dQw4w9WgXcQ/hqdefault.jpg
```

### **Vimeo Örnek:**
```
URL: https://vimeo.com/123456789

Otomatik olu?turulur:
- Embed: <iframe src="https://player.vimeo.com/video/123456789">
```

---

## ?? **Dosya Yap?s?**

```
CoreBuilder/
??? Models/
?   ??? PageContent.cs                  ? ?çerik modeli
?   ??? PageType.cs                     ? Enum'lar
??? Services/
?   ??? PageContentService.cs           ? CRUD servisi
??? Data/
    ??? AppDbContext.cs                 ? PageContents ekli

CoreBuilder.Admin/
??? Pages/
?   ??? PageContentManager.razor        ? ?çerik yöneticisi
?   ??? PageDetail.razor                ? Güncellenmi?
??? Program.cs                          ? Servis kayd? ekli
```

---

## ?? **Database Schema**

```sql
CREATE TABLE PageContents (
    Id GUID PRIMARY KEY,
    PageId GUID NOT NULL,                  -- Hangi sayfa
    ContentType INT NOT NULL,              -- 0=Image, 1=Video, etc.
    Title NVARCHAR(200) NOT NULL,
    Description NVARCHAR(MAX),
    MediaUrl NVARCHAR(500),                -- Foto?raf URL
    ThumbnailUrl NVARCHAR(500),            -- Küçük resim
    EmbedCode NVARCHAR(MAX),               -- Video embed
    LinkUrl NVARCHAR(500),
    LinkText NVARCHAR(100),
    DisplayOrder INT DEFAULT 0,
    IsActive BIT DEFAULT 1,
    MetaData NVARCHAR(MAX),
    CreatedAt DATETIME DEFAULT GETUTCDATE(),
    UpdatedAt DATETIME,
    FOREIGN KEY (PageId) REFERENCES Pages(Id)
)
```

---

## ?? **API Metodlar?**

```csharp
// Tüm içerikleri getir
await ContentService.GetPageContentsAsync(pageId);

// Aktif içerikleri getir
await ContentService.GetActiveContentsAsync(pageId);

// Türe göre getir
await ContentService.GetContentsByTypeAsync(pageId, ContentType.Image);

// Yeni içerik ekle
await ContentService.CreateContentAsync(content);

// Güncelle
await ContentService.UpdateContentAsync(content);

// Sil
await ContentService.DeleteContentAsync(id);

// Yeniden s?rala
await ContentService.ReorderContentsAsync(contentIds);

// ?statistikler
var stats = await ContentService.GetStatisticsAsync(pageId);
```

---

## ?? **CSS Özellikleri**

### **Hover Efekti**
```css
.content-card:hover {
    transform: translateY(-5px);
}
```

### **Lightbox**
```css
.lightbox {
    position: fixed;
    background: rgba(0, 0, 0, 0.9);
    z-index: 2000;
}
```

### **Video Play Button**
```css
.video-thumbnail .play-button {
    position: absolute;
    top: 50%;
    left: 50%;
}
```

---

## ?? **Gelecek Özellikler (Opsiyonel)**

### **Sürükle-B?rak S?ralama**
```razor
@* Sortable.js ile *@
<div id="content-grid" class="sortable">
    @foreach (var content in contents)
    {
        <div data-id="@content.Id">...</div>
    }
</div>
```

### **Toplu ??lemler**
```csharp
// Toplu aktif/pasif
await ContentService.BulkToggleAsync(selectedIds);

// Toplu silme
await ContentService.BulkDeleteAsync(selectedIds);
```

### **Kategoriler**
```csharp
// ?çerik kategorileri
public class ContentCategory
{
    public string Name { get; set; }
    public List<PageContent> Contents { get; set; }
}
```

---

## ? **Checklist**

### **Backend**
- [x] PageContent modeli
- [x] PageContentService
- [x] AppDbContext güncelleme
- [x] ImageService entegrasyonu

### **Frontend**
- [x] PageContentManager.razor
- [x] Foto?raf yükleme formu
- [x] Video embed formu
- [x] ?çerik grid'i
- [x] Lightbox
- [x] ?statistik kartlar?
- [x] PageDetail entegrasyonu

### **Features**
- [x] Image upload
- [x] Video embed (YouTube/Vimeo)
- [x] Thumbnail generation
- [x] Active/Inactive toggle
- [x] Display order
- [x] CRUD operations
- [ ] **Drag & drop reorder** (Gelecek)
- [ ] **Bulk operations** (Gelecek)

---

## ?? **Özet**

### **Eklenmi? Özellikler:**
1. ? **Foto?raf Yükleme** ? Dosya seç, yükle, önizle
2. ? **Video Embed** ? YouTube/Vimeo URL, otomatik embed
3. ? **Lightbox** ? Tam ekran foto?raf görüntüleme
4. ? **Grid Layout** ? Otomatik 4 sütunlu düzen
5. ? **?statistikler** ? Toplam, aktif, tür say?lar?
6. ? **CRUD** ? Ekle, düzenle, sil

### **Kullan?m:**
```
1. Galeri sayfas?na git
2. "?çerikleri Yönet" t?kla
3. Foto?raf/Video ekle
4. Kaydet
5. Otomatik grid'de görüntülenir
```

**TÜM S?STEM HAZIR! Hemen test edin.** ??

---

## ?? **Yard?m**

### **Sorun: Foto?raf yüklenmiyor**
- Dosya boyutu 5MB'dan küçük olmal?
- Sadece JPG, PNG, GIF, WebP

### **Sorun: Video görünmüyor**
- YouTube veya Vimeo linki mi?
- Link do?ru mu kontrol edin

### **Sorun: Lightbox çal??m?yor**
- Sadece foto?raflarda çal???r
- Videolarda çal??maz (tasar?m gere?i)

**Ba?ar?lar!** ??
