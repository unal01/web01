using CoreBuilder.Data;
using CoreBuilder.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CoreBuilder.Factories
{
    public class MarketingSiteFactory
    {
        private readonly AppDbContext _context;

        public MarketingSiteFactory(AppDbContext context)
        {
            _context = context;
        }

        public async Task CreateDefaultPagesAsync(Guid tenantId)
        {
            var pages = new List<Page>
            {
                new Page
                {
                    Id = Guid.NewGuid(),
                    TenantId = tenantId,
                    Title = "Ana Sayfa",
                    Slug = "home",
                    Content = "<h1>Dijital Çözümler</h1><p>İşinizi büyütmek için buradayız.</p>",
                    IsPublished = true
                },
                new Page
                {
                    Id = Guid.NewGuid(),
                    TenantId = tenantId,
                    Title = "Hizmetler",
                    Slug = "services",
                    Content = "<h1>Neler Yapıyoruz?</h1><p>SEO, Sosyal Medya ve Web Tasarım.</p>",
                    IsPublished = true
                },
                new Page
                {
                    Id = Guid.NewGuid(),
                    TenantId = tenantId,
                    Title = "İletişim",
                    Slug = "contact",
                    Content = "<h1>Bize Ulaşın</h1><p>Projenizi konuşalım.</p>",
                    IsPublished = true
                }
            };

            _context.Pages.AddRange(pages);
            await _context.SaveChangesAsync();
        }
    }
}
