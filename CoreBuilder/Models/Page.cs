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
        
        public bool IsPublished { get; set; } = true;

        [Required]
        public required string Content { get; set; }

        public Guid? ParentPageId { get; set; }

        [JsonIgnore]
        [ForeignKey("ParentPageId")]
        public Page? ParentPage { get; set; }
    }
}
