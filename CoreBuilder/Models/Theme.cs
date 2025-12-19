using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace CoreBuilder.Models
{
    public class Theme
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        public required string Name { get; set; }
        
        [Required]
        public required string PrimaryColor { get; set; }
        
        [Required]
        public required string SecondaryColor { get; set; }
        
        [Required]
        public required string FontFamily { get; set; }

        public bool IsDefault { get; set; } = false;

        // İlişki Alanları
        public Guid? TenantId { get; set; }

        // [JsonIgnore] SAYESİNDE 500 HATASI ÇÖZÜLÜR.
        // Bu komut sisteme: "Temayı getirirken, ona bağlı olan Kiracı(Tenant) detaylarını getirme" der.
        [JsonIgnore]
        [ForeignKey("TenantId")]
        public Tenant? Tenant { get; set; }
    }
}
