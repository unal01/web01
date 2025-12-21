using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace CoreBuilder.Models
{
    public class Page
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        public Guid TenantId { get; set; }

        [JsonIgnore]
        [ForeignKey("TenantId")]
        public Tenant? Tenant { get; set; }

        [Required]
        public required string Title { get; set; }

        [Required]
        public required string Slug { get; set; }
        
        /// <summary>
        /// Sayfa türü (Normal, Galeri, Ö?retmen Kadrosu, vb.)
        /// </summary>
        public PageType PageType { get; set; } = PageType.Standard;
        
        /// <summary>
        /// Menü konumu
        /// </summary>
        public MenuLocation MenuLocation { get; set; } = MenuLocation.Main;
        
        /// <summary>
        /// Menüde görünüm s?ras?
        /// </summary>
        public int MenuOrder { get; set; } = 0;
        
        /// <summary>
        /// Üst sayfa (alt menü için)
        /// </summary>
        public Guid? ParentPageId { get; set; }

        [JsonIgnore]
        [ForeignKey("ParentPageId")]
        public Page? ParentPage { get; set; }
        
        /// <summary>
        /// Alt sayfalar
        /// </summary>
        [JsonIgnore]
        public ICollection<Page> ChildPages { get; set; } = new List<Page>();
        
        /// <summary>
        /// Sayfa içerikleri (foto?raflar, videolar, vb.)
        /// </summary>
        [JsonIgnore]
        public ICollection<PageContent> Contents { get; set; } = new List<PageContent>();
        
        /// <summary>
        /// Menüde göster
        /// </summary>
        public bool ShowInMenu { get; set; } = true;
        
        /// <summary>
        /// Menü ikonu
        /// </summary>
        public string? MenuIcon { get; set; }
        
        public bool IsPublished { get; set; } = true;

        [Required]
        public required string Content { get; set; }
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
    }
    
    /// <summary>
    /// Menü konumu
    /// </summary>
    public enum MenuLocation
    {
        /// <summary>
        /// Ana menü (header)
        /// </summary>
        Main = 0,
        
        /// <summary>
        /// Yan menü (sidebar)
        /// </summary>
        Sidebar = 1,
        
        /// <summary>
        /// Alt bilgi (footer)
        /// </summary>
        Footer = 2,
        
        /// <summary>
        /// Gizli (menüde görünmez)
        /// </summary>
        Hidden = 3
    }
    
    public static class MenuLocationExtensions
    {
        public static string GetDisplayName(this MenuLocation location)
        {
            return location switch
            {
                MenuLocation.Main => "?? Ana Menü",
                MenuLocation.Sidebar => "?? Yan Menü",
                MenuLocation.Footer => "?? Alt Bilgi",
                MenuLocation.Hidden => "??? Gizli",
                _ => "Menü"
            };
        }
    }
}
