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

        public async Task<List<Slider>> GetSlidersForPageAsync(Guid tenantId, Guid pageId)
        {
            var all = await GetSlidersAsync(tenantId);
            var pageIdStr = pageId.ToString();
            return all.Where(s => s.ShowOnAllPages || 
                (!string.IsNullOrEmpty(s.PageIds) && s.PageIds.Split(',').Contains(pageIdStr)))
                .ToList();
        }

        public async Task<List<Slider>> GetAllSlidersAsync(Guid tenantId)
        {
            return await _context.Sliders
                .Where(s => s.TenantId == tenantId)
                .OrderBy(s => s.Order)
                .ToListAsync();
        }

        public async Task<Slider?> GetSliderByIdAsync(Guid id)
        {
            return await _context.Sliders.FindAsync(id);
        }

        public async Task<Slider> CreateSliderAsync(Slider slider)
        {
            _context.Sliders.Add(slider);
            await _context.SaveChangesAsync();
            return slider;
        }

        public async Task<Slider?> UpdateSliderAsync(Slider slider)
        {
            var existing = await _context.Sliders.FindAsync(slider.Id);
            if (existing == null) return null;

            existing.Title = slider.Title;
            existing.Description = slider.Description;
            existing.ImageUrl = slider.ImageUrl;
            existing.ButtonText = slider.ButtonText;
            existing.ButtonLink = slider.ButtonLink;
            existing.Order = slider.Order;
            existing.IsActive = slider.IsActive;
            existing.ShowOnAllPages = slider.ShowOnAllPages;
            existing.PageIds = slider.PageIds;

            await _context.SaveChangesAsync();
            return existing;
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

        public async Task<List<Announcement>> GetAnnouncementsForPageAsync(Guid tenantId, Guid pageId)
        {
            var all = await GetAnnouncementsAsync(tenantId);
            var pageIdStr = pageId.ToString();
            return all.Where(a => a.ShowOnAllPages || 
                (!string.IsNullOrEmpty(a.PageIds) && a.PageIds.Split(',').Contains(pageIdStr)))
                .ToList();
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

        public async Task<List<NewsArticle>> GetNewsForPageAsync(Guid tenantId, Guid pageId, int take = 10)
        {
            var all = await _context.NewsArticles
                .Where(n => n.TenantId == tenantId && n.IsPublished)
                .OrderByDescending(n => n.IsFeatured)
                .ThenByDescending(n => n.PublishDate)
                .ToListAsync();
            
            var pageIdStr = pageId.ToString();
            return all.Where(n => n.ShowOnAllPages || 
                (!string.IsNullOrEmpty(n.PageIds) && n.PageIds.Split(',').Contains(pageIdStr)))
                .Take(take)
                .ToList();
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

        // ===== GALERI =====
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
