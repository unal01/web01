using System;
using System.ComponentModel.DataAnnotations;

namespace CoreBuilder.Models
{
    // ===== SLIDER =====
    public class Slider
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();
        
        public Guid TenantId { get; set; }
        public Tenant? Tenant { get; set; }

        [Required]
        public required string Title { get; set; }
        
        public string? Description { get; set; }
        
        [Required]
        public required string ImageUrl { get; set; }
        
        public string? ButtonText { get; set; }
        public string? ButtonLink { get; set; }
        
        public int Order { get; set; } = 0;
        public bool IsActive { get; set; } = true;
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }

    // ===== DUYURULAR =====
    public class Announcement
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();
        
        public Guid TenantId { get; set; }
        public Tenant? Tenant { get; set; }

        [Required]
        public required string Title { get; set; }
        
        [Required]
        public required string Content { get; set; }
        
        public string? ImageUrl { get; set; }
        
        public bool IsImportant { get; set; } = false;
        public bool IsPublished { get; set; } = true;
        
        public DateTime PublishDate { get; set; } = DateTime.UtcNow;
        public DateTime? ExpiryDate { get; set; }
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }

    // ===== HABERLER =====
    public class NewsArticle
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();
        
        public Guid TenantId { get; set; }
        public Tenant? Tenant { get; set; }

        [Required]
        public required string Title { get; set; }
        
        [Required]
        public required string Slug { get; set; }
        
        public string? Summary { get; set; }
        
        [Required]
        public required string Content { get; set; }
        
        public string? FeaturedImageUrl { get; set; }
        
        public string? Author { get; set; }
        public string? Category { get; set; }
        public string? Tags { get; set; }
        
        public int ViewCount { get; set; } = 0;
        public bool IsPublished { get; set; } = true;
        public bool IsFeatured { get; set; } = false;
        
        public DateTime PublishDate { get; set; } = DateTime.UtcNow;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
    }

    // ===== GALERİ ALBÜM =====
    public class GalleryAlbum
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();
        
        public Guid TenantId { get; set; }
        public Tenant? Tenant { get; set; }

        [Required]
        public required string Name { get; set; }
        
        public string? Description { get; set; }
        
        public string? CoverImageUrl { get; set; }
        
        public bool IsPublished { get; set; } = true;
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        
        public ICollection<GalleryImage> Images { get; set; } = new List<GalleryImage>();
    }

    // ===== GALERİ RESİM =====
    public class GalleryImage
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();
        
        public Guid AlbumId { get; set; }
        public GalleryAlbum? Album { get; set; }

        [Required]
        public required string ImageUrl { get; set; }
        
        public string? Title { get; set; }
        public string? Description { get; set; }
        
        public int Order { get; set; } = 0;
        
        public DateTime UploadedAt { get; set; } = DateTime.UtcNow;
    }
}
