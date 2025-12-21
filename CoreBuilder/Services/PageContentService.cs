using CoreBuilder.Data;
using CoreBuilder.Models;
using Microsoft.EntityFrameworkCore;

namespace CoreBuilder.Services;

/// <summary>
/// Sayfa içerik yönetim servisi (foto?raf, video, vb.)
/// </summary>
public class PageContentService
{
    private readonly AppDbContext _context;
    private readonly ILogger<PageContentService> _logger;

    public PageContentService(AppDbContext context, ILogger<PageContentService> logger)
    {
        _context = context;
        _logger = logger;
    }

    /// <summary>
    /// Sayfan?n tüm içeriklerini getir
    /// </summary>
    public async Task<List<PageContent>> GetPageContentsAsync(Guid pageId)
    {
        return await _context.PageContents
            .Where(c => c.PageId == pageId)
            .OrderBy(c => c.DisplayOrder)
            .ThenByDescending(c => c.CreatedAt)
            .ToListAsync();
    }

    /// <summary>
    /// Aktif içerikleri getir
    /// </summary>
    public async Task<List<PageContent>> GetActiveContentsAsync(Guid pageId)
    {
        return await _context.PageContents
            .Where(c => c.PageId == pageId && c.IsActive)
            .OrderBy(c => c.DisplayOrder)
            .ToListAsync();
    }

    /// <summary>
    /// Belirli türdeki içerikleri getir
    /// </summary>
    public async Task<List<PageContent>> GetContentsByTypeAsync(Guid pageId, ContentType contentType)
    {
        return await _context.PageContents
            .Where(c => c.PageId == pageId && c.ContentType == contentType && c.IsActive)
            .OrderBy(c => c.DisplayOrder)
            .ToListAsync();
    }

    /// <summary>
    /// ID'ye göre içerik getir
    /// </summary>
    public async Task<PageContent?> GetContentByIdAsync(Guid id)
    {
        return await _context.PageContents.FindAsync(id);
    }

    /// <summary>
    /// Yeni içerik ekle
    /// </summary>
    public async Task<PageContent> CreateContentAsync(PageContent content)
    {
        // E?er DisplayOrder verilmemi?se, en sona ekle
        if (content.DisplayOrder == 0)
        {
            var maxOrder = await _context.PageContents
                .Where(c => c.PageId == content.PageId)
                .MaxAsync(c => (int?)c.DisplayOrder) ?? 0;
            
            content.DisplayOrder = maxOrder + 1;
        }

        _context.PageContents.Add(content);
        await _context.SaveChangesAsync();
        
        _logger.LogInformation(
            "?çerik eklendi: {Title} (Type: {Type}, PageId: {PageId})", 
            content.Title, 
            content.ContentType, 
            content.PageId);
        
        return content;
    }

    /// <summary>
    /// ?çerik güncelle
    /// </summary>
    public async Task<PageContent?> UpdateContentAsync(PageContent content)
    {
        var existing = await _context.PageContents.FindAsync(content.Id);
        if (existing == null) return null;

        existing.Title = content.Title;
        existing.Description = content.Description;
        existing.MediaUrl = content.MediaUrl;
        existing.ThumbnailUrl = content.ThumbnailUrl;
        existing.EmbedCode = content.EmbedCode;
        existing.LinkUrl = content.LinkUrl;
        existing.LinkText = content.LinkText;
        existing.DisplayOrder = content.DisplayOrder;
        existing.IsActive = content.IsActive;
        existing.MetaData = content.MetaData;
        existing.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();
        
        _logger.LogInformation("?çerik güncellendi: {Title} (Id: {Id})", content.Title, content.Id);
        
        return existing;
    }

    /// <summary>
    /// ?çerik sil
    /// </summary>
    public async Task<bool> DeleteContentAsync(Guid id)
    {
        var content = await _context.PageContents.FindAsync(id);
        if (content == null) return false;

        _context.PageContents.Remove(content);
        await _context.SaveChangesAsync();
        
        _logger.LogInformation("?çerik silindi: {Title} (Id: {Id})", content.Title, id);
        
        return true;
    }

    /// <summary>
    /// ?çerikleri yeniden s?rala
    /// </summary>
    public async Task<bool> ReorderContentsAsync(List<Guid> contentIds)
    {
        for (int i = 0; i < contentIds.Count; i++)
        {
            var content = await _context.PageContents.FindAsync(contentIds[i]);
            if (content != null)
            {
                content.DisplayOrder = i;
                content.UpdatedAt = DateTime.UtcNow;
            }
        }

        await _context.SaveChangesAsync();
        _logger.LogInformation("?çerikler yeniden s?raland?: {Count} ö?e", contentIds.Count);
        
        return true;
    }

    /// <summary>
    /// Aktif/Pasif de?i?tir
    /// </summary>
    public async Task<bool> ToggleActiveStatusAsync(Guid id)
    {
        var content = await _context.PageContents.FindAsync(id);
        if (content == null) return false;

        content.IsActive = !content.IsActive;
        content.UpdatedAt = DateTime.UtcNow;
        await _context.SaveChangesAsync();
        
        _logger.LogInformation(
            "?çerik durumu de?i?tirildi: {Title} -> {Status}", 
            content.Title, 
            content.IsActive ? "Aktif" : "Pasif");
        
        return true;
    }

    /// <summary>
    /// ?statistikler
    /// </summary>
    public async Task<ContentStatistics> GetStatisticsAsync(Guid pageId)
    {
        var contents = await _context.PageContents
            .Where(c => c.PageId == pageId)
            .ToListAsync();

        return new ContentStatistics
        {
            TotalCount = contents.Count,
            ActiveCount = contents.Count(c => c.IsActive),
            InactiveCount = contents.Count(c => !c.IsActive),
            ImageCount = contents.Count(c => c.ContentType == ContentType.Image),
            VideoCount = contents.Count(c => c.ContentType == ContentType.Video),
            TypeCounts = contents
                .GroupBy(c => c.ContentType)
                .ToDictionary(g => g.Key, g => g.Count())
        };
    }
}

/// <summary>
/// ?çerik istatistikleri
/// </summary>
public class ContentStatistics
{
    public int TotalCount { get; set; }
    public int ActiveCount { get; set; }
    public int InactiveCount { get; set; }
    public int ImageCount { get; set; }
    public int VideoCount { get; set; }
    public Dictionary<ContentType, int> TypeCounts { get; set; } = new();
}
