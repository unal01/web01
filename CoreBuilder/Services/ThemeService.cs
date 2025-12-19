using CoreBuilder.Data;
using CoreBuilder.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace CoreBuilder.Services
{
    public class ThemeService
    {
        private readonly AppDbContext _context;

        public ThemeService(AppDbContext context)
        {
            _context = context;
        }

        // --- BLAZOR (Senkron) İçin Metodlar ---
        public List<Theme> GetAllThemes()
        {
            return _context.Themes.ToList();
        }

        // --- API (Asenkron) İçin Metodlar ---

        // 1. Okuma (Get)
        public async Task<List<Theme>> GetThemesAsync()
        {
            return await _context.Themes.ToListAsync();
        }

        public async Task<Theme?> GetThemeAsync(Guid id)
        {
            return await _context.Themes.FindAsync(id);
        }

        // 2. Oluşturma (Create) - EKSİK OLAN BU
        public async Task CreateThemeAsync(Theme theme)
        {
            if (theme.Id == Guid.Empty) theme.Id = Guid.NewGuid();
            _context.Themes.Add(theme);
            await _context.SaveChangesAsync();
        }

        // 3. Silme (Delete) - EKSİK OLAN BU
        public async Task DeleteThemeAsync(Guid id)
        {
            var theme = await _context.Themes.FindAsync(id);
            if (theme != null)
            {
                _context.Themes.Remove(theme);
                await _context.SaveChangesAsync();
            }
        }
    }
}
