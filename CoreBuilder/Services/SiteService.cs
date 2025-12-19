using CoreBuilder.Data;
using CoreBuilder.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks; // Async desteği için gerekli
using Microsoft.EntityFrameworkCore;

namespace CoreBuilder.Services
{
    public class SiteService
    {
        private readonly AppDbContext _context;

        public SiteService(AppDbContext context)
        {
            _context = context;
        }

        // --- BLAZOR (Senkron) İçin Metodlar ---
        public List<Tenant> GetAllSites()
        {
            return _context.Tenants.ToList();
        }

        public void CreateSite(Tenant site)
        {
            if (site.Id == Guid.Empty) site.Id = Guid.NewGuid();
            _context.Tenants.Add(site);
            _context.SaveChanges();
        }

        public void UpdateSite(Tenant site)
        {
            var existing = _context.Tenants.Find(site.Id);
            if (existing != null)
            {
                existing.Name = site.Name;
                existing.Domain = site.Domain;
                existing.Category = site.Category;
                existing.ThemeId = site.ThemeId;
                _context.SaveChanges();
            }
        }

        public void DeleteSite(Guid id)
        {
            var site = _context.Tenants.Find(id);
            if (site != null)
            {
                _context.Tenants.Remove(site);
                _context.SaveChanges();
            }
        }

        // --- API CONTROLLER (Asenkron) İçin Metodlar (Hataları Çözen Kısım) ---

        public async Task<List<Tenant>> GetSitesAsync()
        {
            return await _context.Tenants.ToListAsync();
        }

        public async Task<Tenant?> GetSiteAsync(Guid id)
        {
            return await _context.Tenants.FindAsync(id);
        }

        public async Task CreateSiteAsync(Tenant site)
        {
            if (site.Id == Guid.Empty) site.Id = Guid.NewGuid();
            _context.Tenants.Add(site);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteSiteAsync(Guid id)
        {
            var site = await _context.Tenants.FindAsync(id);
            if (site != null)
            {
                _context.Tenants.Remove(site);
                await _context.SaveChangesAsync();
            }
        }
    }
}
