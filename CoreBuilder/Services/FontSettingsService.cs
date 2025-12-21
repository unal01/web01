using CoreBuilder.Data;
using CoreBuilder.Models;
using Microsoft.EntityFrameworkCore;

namespace CoreBuilder.Services;

/// <summary>
/// Font ayarlar? yönetim servisi
/// </summary>
public class FontSettingsService
{
    private readonly AppDbContext _context;
    private readonly ILogger<FontSettingsService> _logger;

    public FontSettingsService(AppDbContext context, ILogger<FontSettingsService> logger)
    {
        _context = context;
        _logger = logger;
    }

    /// <summary>
    /// Varsay?lan font ayarlar?n? getir veya olu?tur
    /// </summary>
    public async Task<FontSettings> GetOrCreateDefaultAsync()
    {
        var settings = await _context.FontSettings.FirstOrDefaultAsync();
        
        if (settings == null)
        {
            settings = new FontSettings
            {
                PrimaryFont = "Inter, sans-serif",
                HeadingFont = "Inter, sans-serif",
                MonospaceFont = "Consolas, Monaco, 'Courier New', monospace",
                BaseFontSize = 14,
                HeadingFontWeight = "700",
                BodyFontWeight = "400",
                LineHeight = 1.5m,
                LetterSpacing = "normal",
                UseGoogleFonts = true,
                GoogleFontsUrl = PopularFonts.Fonts["Inter"]
            };
            
            _context.FontSettings.Add(settings);
            await _context.SaveChangesAsync();
            
            _logger.LogInformation("Varsay?lan font ayarlar? olu?turuldu");
        }
        
        return settings;
    }

    /// <summary>
    /// Font ayarlar?n? güncelle
    /// </summary>
    public async Task<FontSettings> UpdateAsync(FontSettings settings)
    {
        var existing = await _context.FontSettings.FirstOrDefaultAsync();
        
        if (existing == null)
        {
            _context.FontSettings.Add(settings);
        }
        else
        {
            existing.PrimaryFont = settings.PrimaryFont;
            existing.HeadingFont = settings.HeadingFont;
            existing.MonospaceFont = settings.MonospaceFont;
            existing.BaseFontSize = settings.BaseFontSize;
            existing.HeadingFontWeight = settings.HeadingFontWeight;
            existing.BodyFontWeight = settings.BodyFontWeight;
            existing.LineHeight = settings.LineHeight;
            existing.LetterSpacing = settings.LetterSpacing;
            existing.UseGoogleFonts = settings.UseGoogleFonts;
            existing.GoogleFontsUrl = settings.GoogleFontsUrl;
            existing.UpdatedAt = DateTime.UtcNow;
        }
        
        await _context.SaveChangesAsync();
        _logger.LogInformation("Font ayarlar? güncellendi");
        
        return settings;
    }

    /// <summary>
    /// CSS de?i?kenlerini olu?tur
    /// </summary>
    public string GenerateCssVariables(FontSettings settings)
    {
        return $@"
:root {{
    --font-primary: {settings.PrimaryFont};
    --font-heading: {settings.HeadingFont};
    --font-monospace: {settings.MonospaceFont};
    --font-size-base: {settings.BaseFontSize}px;
    --font-weight-heading: {settings.HeadingFontWeight};
    --font-weight-body: {settings.BodyFontWeight};
    --line-height: {settings.LineHeight};
    --letter-spacing: {settings.LetterSpacing};
}}

body {{
    font-family: var(--font-primary);
    font-size: var(--font-size-base);
    font-weight: var(--font-weight-body);
    line-height: var(--line-height);
    letter-spacing: var(--letter-spacing);
}}

h1, h2, h3, h4, h5, h6 {{
    font-family: var(--font-heading);
    font-weight: var(--font-weight-heading);
}}

code, pre, .font-monospace {{
    font-family: var(--font-monospace);
}}
";
    }

    /// <summary>
    /// Varsay?lan ayarlara s?f?rla
    /// </summary>
    public async Task ResetToDefaultAsync()
    {
        var existing = await _context.FontSettings.FirstOrDefaultAsync();
        if (existing != null)
        {
            _context.FontSettings.Remove(existing);
            await _context.SaveChangesAsync();
        }
        
        await GetOrCreateDefaultAsync();
        _logger.LogInformation("Font ayarlar? varsay?lana s?f?rland?");
    }
}
