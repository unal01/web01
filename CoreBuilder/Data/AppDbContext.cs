using CoreBuilder.Models;
using Microsoft.EntityFrameworkCore;
using System;

namespace CoreBuilder.Data
{
    public class AppDbContext : DbContext
    {
        public Guid? CurrentTenantId { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Tenant> Tenants { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Page> Pages { get; set; }
        public DbSet<CoreBuilder.Models.Module> Modules { get; set; }
        public DbSet<SiteSetting> SiteSettings { get; set; }
        public DbSet<Theme> Themes { get; set; }
        public DbSet<FormDefinition> FormDefinitions { get; set; }
        public DbSet<ImageEntry> Images { get; set; }
        
        // User Management
        public DbSet<User> Users { get; set; }
        public DbSet<UserTenantRole> UserTenantRoles { get; set; }
        
        // System Settings
        public DbSet<SystemSettings> SystemSettings { get; set; }
        public DbSet<FontSettings> FontSettings { get; set; }
        
        // Content Management
        public DbSet<Slider> Sliders { get; set; }
        public DbSet<Announcement> Announcements { get; set; }
        public DbSet<NewsArticle> NewsArticles { get; set; }
        public DbSet<GalleryAlbum> GalleryAlbums { get; set; }
        public DbSet<GalleryImage> GalleryImages { get; set; }
        public DbSet<Teacher> Teachers { get; set; }
        public DbSet<PageContent> PageContents { get; set; }
        
        // Contact Info
        public DbSet<ContactInfo> ContactInfos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Tenant>().HasIndex(t => t.Domain).IsUnique();
            modelBuilder.Entity<User>().HasIndex(u => u.Email).IsUnique();
            modelBuilder.Entity<User>().HasIndex(u => u.Username).IsUnique();
            modelBuilder.Entity<NewsArticle>().HasIndex(n => n.Slug);
            modelBuilder.Entity<ContactInfo>().HasIndex(c => c.TenantId).IsUnique();

            // Query Filters - Multi-Tenant Isolation
            modelBuilder.Entity<Page>().HasQueryFilter(p => !CurrentTenantId.HasValue || p.TenantId == CurrentTenantId);
            modelBuilder.Entity<CoreBuilder.Models.Module>().HasQueryFilter(m => !CurrentTenantId.HasValue || m.TenantId == CurrentTenantId);
            modelBuilder.Entity<SiteSetting>().HasQueryFilter(s => !CurrentTenantId.HasValue || s.TenantId == CurrentTenantId);
            modelBuilder.Entity<Theme>().HasQueryFilter(t => !CurrentTenantId.HasValue || t.TenantId == CurrentTenantId);
            modelBuilder.Entity<FormDefinition>().HasQueryFilter(f => !CurrentTenantId.HasValue || f.TenantId == CurrentTenantId);
            modelBuilder.Entity<ImageEntry>().HasQueryFilter(i => !CurrentTenantId.HasValue || i.TenantId == CurrentTenantId);
            
            // Content Filters
            modelBuilder.Entity<Slider>().HasQueryFilter(s => !CurrentTenantId.HasValue || s.TenantId == CurrentTenantId);
            modelBuilder.Entity<Announcement>().HasQueryFilter(a => !CurrentTenantId.HasValue || a.TenantId == CurrentTenantId);
            modelBuilder.Entity<NewsArticle>().HasQueryFilter(n => !CurrentTenantId.HasValue || n.TenantId == CurrentTenantId);
            modelBuilder.Entity<GalleryAlbum>().HasQueryFilter(g => !CurrentTenantId.HasValue || g.TenantId == CurrentTenantId);
            modelBuilder.Entity<Teacher>().HasQueryFilter(t => !CurrentTenantId.HasValue || t.TenantId == CurrentTenantId);
            modelBuilder.Entity<ContactInfo>().HasQueryFilter(c => !CurrentTenantId.HasValue || c.TenantId == CurrentTenantId);
        }
    }
}
