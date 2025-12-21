using System;
using System.ComponentModel.DataAnnotations;

namespace CoreBuilder.Models
{
    /// <summary>
    /// Site iletisim bilgileri
    /// </summary>
    public class ContactInfo
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        public Guid TenantId { get; set; }
        public Tenant? Tenant { get; set; }

        // Telefon Numaralari
        public string? Phone1 { get; set; }
        public string? Phone1Label { get; set; } = "Telefon";
        
        public string? Phone2 { get; set; }
        public string? Phone2Label { get; set; } = "Telefon 2";
        
        public string? Phone3 { get; set; }
        public string? Phone3Label { get; set; } = "Fax";

        // E-posta
        public string? Email { get; set; }
        public string? Email2 { get; set; }

        // Adres
        public string? Address { get; set; }
        public string? City { get; set; }
        public string? District { get; set; }
        public string? PostalCode { get; set; }

        // Google Maps
        public string? GoogleMapsEmbedUrl { get; set; }
        public string? GoogleMapsLink { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }

        // Sosyal Medya
        public string? WhatsApp { get; set; }
        public string? Instagram { get; set; }
        public string? Facebook { get; set; }
        public string? Twitter { get; set; }
        public string? YouTube { get; set; }
        public string? LinkedIn { get; set; }
        public string? TikTok { get; set; }

        // Calisma Saatleri
        public string? WorkingHours { get; set; }
        public string? WorkingDays { get; set; }

        // Sayfa Duzeni (JSON olarak saklanir)
        public string? PageLayout { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
    }
}
