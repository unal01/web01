using CoreBuilder.Data;
using CoreBuilder.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreBuilder.Services
{
    public class ContentService
    {
        private readonly AppDbContext _context;

        public ContentService(AppDbContext context)
        {
            _context = context;
        }

        // ===== SLIDER =====
        public async Task<List<Slider>> GetSlidersAsync(Guid tenantId)
        {
            return await _context.Sliders
                .Where(s => s.TenantId == tenantId && s.IsActive)
                .OrderBy(s => s.Order)
                .ToListAsync();
        }

        public async Task<Slider> CreateSliderAsync(Slider slider)
        {
            _context.Sliders.Add(slider);
            await _context.SaveChangesAsync();
            return slider;
        }

        public async Task<bool> DeleteSliderAsync(Guid id)
        {
            var slider = await _context.Sliders.FindAsync(id);
            if (slider == null) return false;
            _context.Sliders.Remove(slider);
            await _context.SaveChangesAsync();
            return true;
        }

        // ===== DUYURULAR =====
        public async Task<List<Announcement>> GetAnnouncementsAsync(Guid tenantId)
        {
            return await _context.Announcements
                .Where(a => a.TenantId == tenantId && a.IsPublished)
                .Where(a => !a.ExpiryDate.HasValue || a.ExpiryDate > DateTime.UtcNow)
                .OrderByDescending(a => a.IsImportant)
                .ThenByDescending(a => a.PublishDate)
                .ToListAsync();
        }

        public async Task<Announcement> CreateAnnouncementAsync(Announcement announcement)
        {
            _context.Announcements.Add(announcement);
            await _context.SaveChangesAsync();
            return announcement;
        }

        public async Task<bool> DeleteAnnouncementAsync(Guid id)
        {
            var announcement = await _context.Announcements.FindAsync(id);
            if (announcement == null) return false;
            _context.Announcements.Remove(announcement);
            await _context.SaveChangesAsync();
            return true;
        }

        // ===== HABERLER =====
        public async Task<List<NewsArticle>> GetNewsArticlesAsync(Guid tenantId, int take = 10)
        {
            return await _context.NewsArticles
                .Where(n => n.TenantId == tenantId && n.IsPublished)
                .OrderByDescending(n => n.IsFeatured)
                .ThenByDescending(n => n.PublishDate)
                .Take(take)
                .ToListAsync();
        }

        public async Task<NewsArticle?> GetNewsBySlugAsync(Guid tenantId, string slug)
        {
            var article = await _context.NewsArticles
                .FirstOrDefaultAsync(n => n.TenantId == tenantId && n.Slug == slug);
            
            if (article != null)
            {
                article.ViewCount++;
                await _context.SaveChangesAsync();
            }
            
            return article;
        }

        public async Task<NewsArticle> CreateNewsArticleAsync(NewsArticle article)
        {
            _context.NewsArticles.Add(article);
            await _context.SaveChangesAsync();
            return article;
        }

        public async Task<bool> DeleteNewsArticleAsync(Guid id)
        {
            var article = await _context.NewsArticles.FindAsync(id);
            if (article == null) return false;
            _context.NewsArticles.Remove(article);
            await _context.SaveChangesAsync();
            return true;
        }

        // ===== GALERİ =====
        public async Task<List<GalleryAlbum>> GetGalleryAlbumsAsync(Guid tenantId)
        {
            return await _context.GalleryAlbums
                .Include(g => g.Images)
                .Where(g => g.TenantId == tenantId && g.IsPublished)
                .OrderByDescending(g => g.CreatedAt)
                .ToListAsync();
        }

        public async Task<GalleryAlbum?> GetAlbumWithImagesAsync(Guid albumId)
        {
            return await _context.GalleryAlbums
                .Include(g => g.Images.OrderBy(i => i.Order))
                .FirstOrDefaultAsync(g => g.Id == albumId);
        }

        public async Task<GalleryAlbum> CreateAlbumAsync(GalleryAlbum album)
        {
            _context.GalleryAlbums.Add(album);
            await _context.SaveChangesAsync();
            return album;
        }

        public async Task<GalleryImage> AddImageToAlbumAsync(GalleryImage image)
        {
            _context.GalleryImages.Add(image);
            await _context.SaveChangesAsync();
            return image;
        }

        public async Task<bool> DeleteAlbumAsync(Guid id)
        {
            var album = await _context.GalleryAlbums.Include(g => g.Images).FirstOrDefaultAsync(g => g.Id == id);
            if (album == null) return false;
            _context.GalleryAlbums.Remove(album);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
