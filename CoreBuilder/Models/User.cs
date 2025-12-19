using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CoreBuilder.Models
{
    public class User
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        public required string Username { get; set; }

        [Required]
        [EmailAddress]
        public required string Email { get; set; }

        [Required]
        public required string PasswordHash { get; set; }

        public string? FullName { get; set; }

        public bool IsActive { get; set; } = true;

        public bool IsSuperAdmin { get; set; } = false;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime? LastLoginAt { get; set; }

        // İlişkiler
        public ICollection<UserTenantRole> TenantRoles { get; set; } = new List<UserTenantRole>();
    }

    public class UserTenantRole
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        public Guid UserId { get; set; }
        public User? User { get; set; }

        public Guid TenantId { get; set; }
        public Tenant? Tenant { get; set; }

        [Required]
        public required string Role { get; set; } // Admin, Editor, Viewer

        public DateTime AssignedAt { get; set; } = DateTime.UtcNow;
    }

    public static class Roles
    {
        public const string SuperAdmin = "SuperAdmin";
        public const string TenantAdmin = "TenantAdmin";
        public const string Editor = "Editor";
        public const string Viewer = "Viewer";
    }
}
