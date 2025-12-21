using CoreBuilder.Data;
using CoreBuilder.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CoreBuilder.Factories
{
    public class ExamPrepSiteFactory
    {
        private readonly AppDbContext _context;

        public ExamPrepSiteFactory(AppDbContext context)
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
                    Title = "Anasayfa",
                    Slug = "anasayfa",
                    Content = "<h1>Sinav Hazirlik Merkezi</h1><p>KPSS, ALES, DGS ve tum sinavlar icin uzman egitmenlerle basariya ulasin!</p>",
                    IsPublished = true,
                    ShowInMenu = true,
                    MenuOrder = 0
                },
                new Page
                {
                    Id = Guid.NewGuid(),
                    TenantId = tenantId,
                    Title = "Hakkimizda",
                    Slug = "hakkimizda",
                    Content = "<h1>Hakkimizda</h1><p>15 yillik tecrubeyle egitim veriyoruz.</p>",
                    IsPublished = true,
                    ShowInMenu = true,
                    MenuOrder = 1
                },
                new Page
                {
                    Id = Guid.NewGuid(),
                    TenantId = tenantId,
                    Title = "Kurslarimiz",
                    Slug = "kurslar",
                    Content = "<h1>Kurslarimiz</h1><ul><li>KPSS Hazirlik</li><li>ALES Hazirlik</li><li>DGS Hazirlik</li></ul>",
                    IsPublished = true,
                    ShowInMenu = true,
                    MenuOrder = 2
                },
                new Page
                {
                    Id = Guid.NewGuid(),
                    TenantId = tenantId,
                    Title = "Iletisim",
                    Slug = "iletisim",
                    Content = "<h1>Iletisim</h1><p>Bize ulasin: 0850 XXX XX XX</p>",
                    IsPublished = true,
                    ShowInMenu = true,
                    MenuOrder = 3
                }
            };

            _context.Pages.AddRange(pages);
            await _context.SaveChangesAsync();
        }
    }
}
