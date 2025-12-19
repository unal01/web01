using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace CoreBuilder.Models
{
    public class Module
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        public Guid TenantId { get; set; }

        [JsonIgnore]
        [ForeignKey("TenantId")]
        public Tenant? Tenant { get; set; }

        [Required, MaxLength(200)]
        public required string Name { get; set; }

        public required string ConfigurationJson { get; set; } = "{}";

        public bool Enabled { get; set; } = true;
    }
}
