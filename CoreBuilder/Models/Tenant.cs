using System;
using System.ComponentModel.DataAnnotations;

namespace CoreBuilder.Models
{
    public class Tenant
    {
        public Guid Id { get; set; }

        [Required]
        public string Name { get; set; } = string.Empty;

        [Required]
        public string Domain { get; set; } = string.Empty;

        public string Category { get; set; } = "Education";

        public Guid ThemeId { get; set; }

        /// <summary>
        /// Site logosu URL veya Base64
        /// </summary>
        public string? LogoUrl { get; set; }

        /// <summary>
        /// Favicon URL veya Base64
        /// </summary>
        public string? FaviconUrl { get; set; }

        /// <summary>
        /// Header arka plan rengi
        /// </summary>
        public string HeaderColor { get; set; } = "#ffffff";

        /// <summary>
        /// Header yazi rengi
        /// </summary>
        public string HeaderTextColor { get; set; } = "#333333";
    }
}
