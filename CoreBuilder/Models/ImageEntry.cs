using System;
using System.ComponentModel.DataAnnotations;

namespace CoreBuilder.Models
{
    public class ImageEntry
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();
        public string FileName { get; set; } = string.Empty;
        public string FilePath { get; set; } = string.Empty; // Örn: /uploads/resim.jpg
        public DateTime UploadDate { get; set; } = DateTime.Now;

        // Hangi siteye (Tenant) ait olduğunu takip etmek için
        public Guid TenantId { get; set; }
    }
}
