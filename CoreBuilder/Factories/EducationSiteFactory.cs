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
                    Slug = "home",
                    Content = "<h1>Eğitim Kurumumuza Hoşgeldiniz</h1><p>Geleceğinizi bizimle şekillendirin.</p>",
                    IsPublished = true
                },
                new Page
                {
                    Id = Guid.NewGuid(),
                    TenantId = tenantId,
                    Title = "Hakkımızda",
                    Slug = "about",
                    Content = "<h1>Biz Kimiz?</h1><p>20 yıldır eğitim sektöründe öncüyüz.</p>",
                    IsPublished = true
                },
                new Page
                {
                    Id = Guid.NewGuid(),
                    TenantId = tenantId,
                    Title = "Kurslar",
                    Slug = "courses",
                    Content = "<h1>Eğitimlerimiz</h1><ul><li>Matematik</li><li>Fen Bilimleri</li></ul>",
                    IsPublished = true
                }
            };

            _context.Pages.AddRange(pages);
            await _context.SaveChangesAsync();
        }
    }
}
