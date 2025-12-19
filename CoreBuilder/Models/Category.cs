using System;
using System.ComponentModel.DataAnnotations;

namespace CoreBuilder.Models
{
    public class Category
    {
        [Key]
        public int Id { get; set; }

        [Required, MaxLength(100)]
        public required string Name { get; set; }

        [MaxLength(500)]
        public required string Description { get; set; }
    }
}
