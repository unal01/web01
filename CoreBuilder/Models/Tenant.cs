using System;
using System.ComponentModel.DataAnnotations;

namespace CoreBuilder.Models
{
    public class Tenant
    {
        public Guid Id { get; set; }

        [Required]
        public string Name { get; set; } = string.Empty; // "SiteName" yerine "Name"

        [Required]
        public string Domain { get; set; } = string.Empty;

        public string Category { get; set; } = "Education"; // "CategoryType" yerine "Category"

        public Guid ThemeId { get; set; } // Bu özellik eksikti, eklendi.
    }
}
