using CoreBuilder.Data;
using CoreBuilder.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CoreBuilder.Factories
{
    public class EducationSiteFactory
    {
        private readonly AppDbContext _context;

        public EducationSiteFactory(AppDbContext context)
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
                    Content = "<h1>Hosgeldiniz</h1><p>Egitim kurumumuza hosgeldiniz.</p>",
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
                    Content = "<h1>Biz Kimiz?</h1><p>20 yildir egitim sektorunde oncuyuz.</p>",
                    IsPublished = true,
                    ShowInMenu = true,
                    MenuOrder = 1
                },
                new Page
                {
                    Id = Guid.NewGuid(),
                    TenantId = tenantId,
                    Title = "Kurslar",
                    Slug = "kurslar",
                    Content = "<h1>Egitimlerimiz</h1><ul><li>Matematik</li><li>Fen Bilimleri</li></ul>",
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
                    Content = "<h1>Iletisim</h1><p>Bize ulasin.</p>",
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
