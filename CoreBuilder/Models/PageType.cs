namespace CoreBuilder.Models;

/// <summary>
/// Sayfa turleri
/// </summary>
public enum PageType
{
    /// <summary>
    /// Normal icerik sayfasi (Hakkimizda, Iletisim, vb.)
    /// </summary>
    Standard = 0,
    
    /// <summary>
    /// Fotograf galerisi
    /// </summary>
    PhotoGallery = 1,
    
    /// <summary>
    /// Video galerisi
    /// </summary>
    VideoGallery = 2,
    
    /// <summary>
    /// Ogretmen kadrosu
    /// </summary>
    Teachers = 3,
    
    /// <summary>
    /// Personel listesi
    /// </summary>
    Staff = 4,
    
    /// <summary>
    /// Iletisim formu
    /// </summary>
    Contact = 5,
    
    /// <summary>
    /// Duyurular
    /// </summary>
    Announcements = 6,
    
    /// <summary>
    /// Haberler
    /// </summary>
    News = 7,
    
    /// <summary>
    /// Slider
    /// </summary>
    Slider = 8
}

/// <summary>
/// PageType uzanti metodlari
/// </summary>
public static class PageTypeExtensions
{
    public static string GetDisplayName(this PageType type)
    {
        return type switch
        {
            PageType.Standard => "Normal Sayfa",
            PageType.PhotoGallery => "Fotograf Galerisi",
            PageType.VideoGallery => "Video Galerisi",
            PageType.Teachers => "Ogretmen Kadrosu",
            PageType.Staff => "Personel Listesi",
            PageType.Contact => "Iletisim Formu",
            PageType.Announcements => "Duyurular",
            PageType.News => "Haberler",
            PageType.Slider => "Slider",
            _ => "Sayfa"
        };
    }

    public static string GetDescription(this PageType type)
    {
        return type switch
        {
            PageType.Standard => "HTML icerigi olan klasik sayfa",
            PageType.PhotoGallery => "Fotograf albumleri ve resimler",
            PageType.VideoGallery => "Video koleksiyonlari (YouTube, Vimeo)",
            PageType.Teachers => "Ogretmen profilleri ve bilgileri",
            PageType.Staff => "Personel listesi ve gorevleri",
            PageType.Contact => "Iletisim formu ve bilgileri",
            PageType.Announcements => "Duyuru listesi",
            PageType.News => "Haber icerikleri",
            PageType.Slider => "Ana sayfa slider yonetimi",
            _ => ""
        };
    }

    public static string GetIcon(this PageType type)
    {
        return type switch
        {
            PageType.Standard => "bi-file-text",
            PageType.PhotoGallery => "bi-images",
            PageType.VideoGallery => "bi-play-btn",
            PageType.Teachers => "bi-people",
            PageType.Staff => "bi-person-badge",
            PageType.Contact => "bi-envelope",
            PageType.Announcements => "bi-megaphone",
            PageType.News => "bi-newspaper",
            PageType.Slider => "bi-sliders",
            _ => "bi-file"
        };
    }
}
