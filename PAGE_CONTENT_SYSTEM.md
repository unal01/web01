# ?? Sayfa ?çerik Yönetim Sistemi - Komple K?lavuz

## ? Eklenen Özellikler

### 1. **Yeni Modeller**

#### **PageContent** (CoreBuilder/Models/PageContent.cs)
Sayfalara dinamik içerik eklemek için:
```csharp
- Title (Ba?l?k)
- Description (Aç?klama)
- MediaUrl (Foto?raf/Video URL)
- ThumbnailUrl (Küçük resim)
- EmbedCode (YouTube embed kodu)
- LinkUrl + LinkText (Buton linki)
- DisplayOrder (S?ralama)
- IsActive (Aktif/Pasif)
- ContentType (Image, Video, Slider, Text, File, TeacherProfile, StaffProfile)
```

#### **Page Modeli Güncellemeleri**
```csharp
- PageType (Sayfa türü)
- MenuLocation (Ana/Yan/Alt menü)
- MenuOrder (Menü s?ras?)
- ParentPageId (Üst sayfa)
- ShowInMenu (Menüde göster/gizle)
- MenuIcon (Menü ikonu)
- Contents (Sayfa içerikleri koleksiyonu)
```

### 2. **Enum Türleri**

#### **PageType**
- ?? Standard (Normal sayfa)
- ??? PhotoGallery (Foto?raf galerisi)
- ?? VideoGallery (Video galerisi)
- ????? Teachers (Ö?retmen kadrosu)
- ?? Staff (Personel)
- ?? Contact (?leti?im)
- ?? Announcements (Duyurular)
- ?? News (Haberler)
- ??? Slider

#### **ContentType**
- ??? Image (Foto?raf)
- ?? Video (Video)
- ??? SliderItem (Slider ö?esi)
- ?? TextBlock (Metin blo?u)
- ?? File (Dosya)
- ????? TeacherProfile (Ö?retmen profili)
- ?? StaffProfile (Personel profili)

#### **MenuLocation**
- ?? Main (Ana menü)
- ?? Sidebar (Yan menü)
- ?? Footer (Alt bilgi)
- ?? Hidden (Gizli)

---

## ?? Kullan?m Senaryolar?

### **Senaryo 1: Foto?raf Galerisi Olu?turma**

1. **Sayfa Olu?tur**
   - Ba?l?k: "Foto Galeri"
   - Sayfa Türü: ??? Foto?raf Galerisi
   - Menü: Ana Menü

2. **?çerik Ekle**
   - Tür: ??? Foto?raf
   - Ba?l?k: "2024 Mezuniyet Töreni"
   - Medya URL: https://example.com/photo1.jpg
   - Aç?klama: "Mezuniyet töreni an?lar?"

3. **S?ralama**
   - DisplayOrder ile s?ra ayarlay?n (0, 1, 2...)

### **Senaryo 2: Video Galerisi**

1. **Sayfa Olu?tur**
   - Ba?l?k: "Tan?t?m Videolar?"
   - Sayfa Türü: ?? Video Galerisi

2. **?çerik Ekle**
   - Tür: ?? Video
   - Ba?l?k: "Kampüs Tan?t?m Filmi"
   - EmbedCode: `<iframe src="https://youtube.com/embed/xxxxx"></iframe>`
   - Thumbnail: https://img.youtube.com/vi/xxxxx/hqdefault.jpg

### **Senaryo 3: Alt Menü Yap?s?**

```
?? Kurumsal (Ana Menü)
   ?? Hakk?m?zda (Alt sayfa)
   ?? Tarihçe (Alt sayfa)
   ?? Yönetim (Alt sayfa)
```

**Kurumsal sayfas?:**
- ParentPageId: null
- MenuLocation: Main
- MenuOrder: 1

**Hakk?m?zda sayfas?:**
- ParentPageId: {Kurumsal sayfas? ID}
- MenuLocation: Main
- MenuOrder: 0

---

## ??? Kod Örnekleri

### **PageContent Ekleme**

```csharp
// PageContentService.cs (olu?turulacak)
public async Task<PageContent> AddContentAsync(Guid pageId, PageContent content)
{
    content.PageId = pageId;
    _context.PageContents.Add(content);
    await _context.SaveChangesAsync();
    return content;
}
```

### **Sayfa ?çeriklerini Getirme**

```csharp
public async Task<List<PageContent>> GetPageContentsAsync(Guid pageId)
{
    return await _context.PageContents
        .Where(c => c.PageId == pageId && c.IsActive)
        .OrderBy(c => c.DisplayOrder)
        .ToListAsync();
}
```

### **Foto?raf Ekleme Örne?i**

```csharp
var photo = new PageContent
{
    PageId = galleryPageId,
    ContentType = ContentType.Image,
    Title = "Kampüs Görünümü",
    Description = "Ana giri? kap?s?",
    MediaUrl = "https://example.com/campus.jpg",
    ThumbnailUrl = "https://example.com/campus-thumb.jpg",
    DisplayOrder = 0,
    IsActive = true
};

await pageContentService.AddContentAsync(galleryPageId, photo);
```

### **Video Ekleme Örne?i**

```csharp
var video = new PageContent
{
    PageId = videoPageId,
    ContentType = ContentType.Video,
    Title = "Tan?t?m Videosu",
    Description = "Üniversitemizin tan?t?m filmi",
    EmbedCode = "<iframe width='560' height='315' src='https://www.youtube.com/embed/dQw4w9WgXcQ'></iframe>",
    ThumbnailUrl = "https://img.youtube.com/vi/dQw4w9WgXcQ/hqdefault.jpg",
    DisplayOrder = 0,
    IsActive = true
};
```

---

## ?? UI Bile?enleri (Olu?turulacak)

### **1. PageContentManager.razor**
```razor
@page "/page/{PageId:guid}/contents"

<h3>?çerik Yöneticisi</h3>

@* ?çerik türü seçimi *@
<select @bind="selectedContentType">
    @foreach (ContentType type in Enum.GetValues(typeof(ContentType)))
    {
        <option value="@type">@type.GetDisplayName()</option>
    }
</select>

@* ?çerik formu *@
@if (selectedContentType == ContentType.Image)
{
    <PhotoUploadForm PageId="@PageId" />
}
else if (selectedContentType == ContentType.Video)
{
    <VideoEmbedForm PageId="@PageId" />
}

@* ?çerik listesi *@
<ContentList PageId="@PageId" />
```

### **2. ContentList.razor**
```razor
<div class="row g-3">
    @foreach (var content in contents)
    {
        <div class="col-md-4">
            <div class="card">
                <img src="@(content.ThumbnailUrl ?? content.MediaUrl)" />
                <div class="card-body">
                    <h5>@content.Title</h5>
                    <p>@content.Description</p>
                    <button @onclick="() => EditContent(content.Id)">Düzenle</button>
                    <button @onclick="() => DeleteContent(content.Id)">Sil</button>
                </div>
            </div>
        </div>
    }
</div>
```

---

## ?? Migrasyon (InMemory DB için gerekmiyor)

E?er SQL Server kullanacaksan?z:

```bash
dotnet ef migrations add AddPageContentSystem
dotnet ef database update
```

---

## ?? Örnek Sayfa Render'?

```razor
@* PublicPage.razor güncellemesi *@

@if (currentPage.PageType == PageType.PhotoGallery)
{
    <div class="photo-gallery">
        @foreach (var photo in currentPage.Contents.Where(c => c.ContentType == ContentType.Image))
        {
            <div class="gallery-item">
                <img src="@photo.MediaUrl" alt="@photo.Title" />
                <h4>@photo.Title</h4>
                <p>@photo.Description</p>
            </div>
        }
    </div>
}
else if (currentPage.PageType == PageType.VideoGallery)
{
    <div class="video-gallery">
        @foreach (var video in currentPage.Contents.Where(c => c.ContentType == ContentType.Video))
        {
            <div class="video-item">
                @((MarkupString)video.EmbedCode)
                <h4>@video.Title</h4>
                <p>@video.Description</p>
            </div>
        }
    </div>
}
```

---

## ? Yap?lacaklar Listesi

### **Backend (Tamamland?)**
- [x] PageContent modeli
- [x] Page modeli güncellemeleri
- [x] Enum türleri
- [x] AppDbContext güncelleme

### **Backend (Yap?lacak)**
- [ ] PageContentService olu?tur
- [ ] CRUD operasyonlar?
- [ ] S?ralama/Yeniden s?ralama
- [ ] Toplu i?lem metodlar?

### **Frontend (Yap?lacak)**
- [ ] PageContentManager.razor
- [ ] PhotoUploadForm.razor
- [ ] VideoEmbedForm.razor
- [ ] ContentList.razor
- [ ] SliderManager.razor
- [ ] Sürükle-b?rak s?ralama

### **Integration (Yap?lacak)**
- [ ] Pages.razor'a içerik yönetimi butonu
- [ ] PageDetail.razor'a içerik sekmesi
- [ ] PublicPage.razor'da içerik render
- [ ] Menü yap?s? render

---

## ?? H?zl? Ba?lang?ç

### **1. Servisi Kaydet**

`Program.cs`:
```csharp
builder.Services.AddScoped<PageContentService>();
```

### **2. ?çerik Ekle**

```csharp
// Pages.razor'da "?çerikleri Yönet" butonu ekle
<button @onclick="() => NavigateToContents(page.Id)">
    <span class="oi oi-layers"></span> ?çerikleri Yönet
</button>
```

### **3. Test Et**

1. Sayfa olu?tur (Foto?raf Galerisi)
2. ?çerik ekle (3-4 foto?raf)
3. Önizle
4. S?rala
5. Yay?nla

---

## ?? Database Schema

```
???????????????          ????????????????
?    Pages    ??????????<? PageContents ?
???????????????          ????????????????
? Id          ?          ? Id           ?
? TenantId    ?          ? PageId       ? FK
? Title       ?          ? ContentType  ?
? PageType    ?          ? Title        ?
? MenuLocation?          ? MediaUrl     ?
? ParentPageId? FK       ? EmbedCode    ?
? MenuOrder   ?          ? DisplayOrder ?
???????????????          ????????????????
       ?
       ? Self-reference
       ????????
```

---

## ?? Sonuç

Bu sistem ile:
- ? Dinamik sayfa içerikleri
- ? Foto?raf/Video galeriler
- ? Slider yönetimi
- ? Hiyerar?ik menü yap?s?
- ? Sürükle-b?rak s?ralama
- ? Çoklu içerik türleri

**Tüm modeller haz?r! ?imdi UI olu?turup entegre edin.** ??

---

## ?? Yard?m

Sorular:
1. PageContentService nas?l olu?turulur?
2. UI bile?enleri nas?l entegre edilir?
3. Sürükle-b?rak nas?l implement edilir?

**Sonraki ad?m: PageContentService olu?turma ve UI entegrasyonu**
