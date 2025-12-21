using CoreBuilder.Data;
using CoreBuilder.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreBuilder.Services;

public class SiteTemplateService
{
    private readonly AppDbContext _context;

    public SiteTemplateService(AppDbContext context)
    {
        _context = context;
    }

    public List<SiteTemplate> GetAllTemplates()
    {
        return new List<SiteTemplate>
        {
            GetEducationTemplate(),
            GetCorporateTemplate(),
            GetECommerceTemplate(),
            GetBlogTemplate(),
            GetPortfolioTemplate()
        };
    }

    private SiteTemplate GetEducationTemplate()
    {
        return new SiteTemplate
        {
            Id = "education",
            Name = "Egitim Kurumu",
            Description = "Okul, universite, kurs merkezi icin hazir icerik yapisi",
            Category = "Education",
            Icon = "oi-book",
            Pages = new List<PageTemplateDefinition>
            {
                new PageTemplateDefinition
                {
                    Title = "Anasayfa",
                    Slug = "anasayfa",
                    PageType = PageType.Standard,
                    MenuOrder = 0,
                    Widgets = new List<WidgetDefinition>
                    {
                        new WidgetDefinition { Type = ContentType.WidgetSlider, Title = "Ana Slider", DisplayOrder = 0 },
                        new WidgetDefinition { Type = ContentType.WidgetAnnouncements, Title = "Duyurular", DisplayOrder = 1 }
                    },
                    DefaultContent = "<h1>Hosgeldiniz</h1><p>Egitim kurumumuza hosgeldiniz.</p>"
                },
                new PageTemplateDefinition
                {
                    Title = "Kurumsal",
                    Slug = "kurumsal",
                    PageType = PageType.Standard,
                    MenuOrder = 1,
                    DefaultContent = "<h1>Kurumsal</h1>"
                },
                new PageTemplateDefinition
                {
                    Title = "Hakkimizda",
                    Slug = "hakkimizda",
                    PageType = PageType.Standard,
                    MenuOrder = 0,
                    ParentSlug = "kurumsal",
                    DefaultContent = "<h1>Hakkimizda</h1><p>Biz kimiz, ne yapiyoruz...</p>"
                },
                new PageTemplateDefinition
                {
                    Title = "Vizyon Misyon",
                    Slug = "vizyon-misyon",
                    PageType = PageType.Standard,
                    MenuOrder = 1,
                    ParentSlug = "kurumsal",
                    DefaultContent = "<h1>Vizyon ve Misyon</h1>"
                },
                new PageTemplateDefinition
                {
                    Title = "Ogretmen Kadrosu",
                    Slug = "ogretmenler",
                    PageType = PageType.Teachers,
                    MenuOrder = 2,
                    ParentSlug = "kurumsal",
                    DefaultContent = ""
                },
                new PageTemplateDefinition
                {
                    Title = "Galeri",
                    Slug = "galeri",
                    PageType = PageType.PhotoGallery,
                    MenuOrder = 2,
                    Widgets = new List<WidgetDefinition>
                    {
                        new WidgetDefinition { Type = ContentType.Image, Title = "Fotograf Galerisi", DisplayOrder = 0 }
                    }
                },
                new PageTemplateDefinition
                {
                    Title = "Duyurular",
                    Slug = "duyurular",
                    PageType = PageType.Announcements,
                    MenuOrder = 3
                },
                new PageTemplateDefinition
                {
                    Title = "Iletisim",
                    Slug = "iletisim",
                    PageType = PageType.Contact,
                    MenuOrder = 4,
                    DefaultContent = "<h1>Iletisim</h1><p>Bize ulasin.</p>"
                }
            }
        };
    }

    private SiteTemplate GetCorporateTemplate()
    {
        return new SiteTemplate
        {
            Id = "corporate",
            Name = "Kurumsal Firma",
            Description = "Sirket, kurum ve kuruluslar icin profesyonel yapi",
            Category = "Corporate",
            Icon = "oi-briefcase",
            Pages = new List<PageTemplateDefinition>
            {
                new PageTemplateDefinition
                {
                    Title = "Anasayfa",
                    Slug = "anasayfa",
                    PageType = PageType.Standard,
                    MenuOrder = 0,
                    Widgets = new List<WidgetDefinition>
                    {
                        new WidgetDefinition { Type = ContentType.WidgetSlider, Title = "Slider", DisplayOrder = 0 }
                    }
                },
                new PageTemplateDefinition
                {
                    Title = "Hakkimizda",
                    Slug = "hakkimizda",
                    PageType = PageType.Standard,
                    MenuOrder = 1
                },
                new PageTemplateDefinition
                {
                    Title = "Hizmetlerimiz",
                    Slug = "hizmetler",
                    PageType = PageType.Standard,
                    MenuOrder = 2
                },
                new PageTemplateDefinition
                {
                    Title = "Referanslar",
                    Slug = "referanslar",
                    PageType = PageType.PhotoGallery,
                    MenuOrder = 3
                },
                new PageTemplateDefinition
                {
                    Title = "Iletisim",
                    Slug = "iletisim",
                    PageType = PageType.Contact,
                    MenuOrder = 4
                }
            }
        };
    }

    private SiteTemplate GetECommerceTemplate()
    {
        return new SiteTemplate
        {
            Id = "ecommerce",
            Name = "E-Ticaret",
            Description = "Online magaza, satis sitesi",
            Category = "ECommerce",
            Icon = "oi-cart",
            Pages = new List<PageTemplateDefinition>
            {
                new PageTemplateDefinition
                {
                    Title = "Anasayfa",
                    Slug = "anasayfa",
                    PageType = PageType.Standard,
                    MenuOrder = 0,
                    Widgets = new List<WidgetDefinition>
                    {
                        new WidgetDefinition { Type = ContentType.WidgetSlider, Title = "Kampanyalar", DisplayOrder = 0 }
                    }
                },
                new PageTemplateDefinition
                {
                    Title = "Urunler",
                    Slug = "urunler",
                    PageType = PageType.Standard,
                    MenuOrder = 1
                },
                new PageTemplateDefinition
                {
                    Title = "Sepet",
                    Slug = "sepet",
                    PageType = PageType.Standard,
                    MenuOrder = 2
                },
                new PageTemplateDefinition
                {
                    Title = "Hakkimizda",
                    Slug = "hakkimizda",
                    PageType = PageType.Standard,
                    MenuOrder = 3
                },
                new PageTemplateDefinition
                {
                    Title = "Iletisim",
                    Slug = "iletisim",
                    PageType = PageType.Contact,
                    MenuOrder = 4
                }
            }
        };
    }

    private SiteTemplate GetBlogTemplate()
    {
        return new SiteTemplate
        {
            Id = "blog",
            Name = "Blog / Haber Sitesi",
            Description = "Icerik odakli blog veya haber platformu",
            Category = "Blog",
            Icon = "oi-rss",
            Pages = new List<PageTemplateDefinition>
            {
                new PageTemplateDefinition
                {
                    Title = "Anasayfa",
                    Slug = "anasayfa",
                    PageType = PageType.Standard,
                    MenuOrder = 0,
                    Widgets = new List<WidgetDefinition>
                    {
                        new WidgetDefinition { Type = ContentType.WidgetNews, Title = "Son Yazilar", DisplayOrder = 0 }
                    }
                },
                new PageTemplateDefinition
                {
                    Title = "Yazilar",
                    Slug = "yazilar",
                    PageType = PageType.Standard,
                    MenuOrder = 1
                },
                new PageTemplateDefinition
                {
                    Title = "Kategoriler",
                    Slug = "kategoriler",
                    PageType = PageType.Standard,
                    MenuOrder = 2
                },
                new PageTemplateDefinition
                {
                    Title = "Hakkimda",
                    Slug = "hakkimda",
                    PageType = PageType.Standard,
                    MenuOrder = 3
                },
                new PageTemplateDefinition
                {
                    Title = "Iletisim",
                    Slug = "iletisim",
                    PageType = PageType.Contact,
                    MenuOrder = 4
                }
            }
        };
    }

    private SiteTemplate GetPortfolioTemplate()
    {
        return new SiteTemplate
        {
            Id = "portfolio",
            Name = "Portfolyo / Kisisel Site",
            Description = "Freelancer, tasarimci, fotografci icin",
            Category = "Portfolio",
            Icon = "oi-image",
            Pages = new List<PageTemplateDefinition>
            {
                new PageTemplateDefinition
                {
                    Title = "Anasayfa",
                    Slug = "anasayfa",
                    PageType = PageType.Standard,
                    MenuOrder = 0
                },
                new PageTemplateDefinition
                {
                    Title = "Portfolyo",
                    Slug = "portfolyo",
                    PageType = PageType.PhotoGallery,
                    MenuOrder = 1
                },
                new PageTemplateDefinition
                {
                    Title = "Hakkimda",
                    Slug = "hakkimda",
                    PageType = PageType.Standard,
                    MenuOrder = 2
                },
                new PageTemplateDefinition
                {
                    Title = "Iletisim",
                    Slug = "iletisim",
                    PageType = PageType.Contact,
                    MenuOrder = 3
                }
            }
        };
    }

    public async Task CreateSiteFromTemplate(Guid tenantId, string templateId)
    {
        var template = GetAllTemplates().FirstOrDefault(t => t.Id == templateId);
        if (template == null) return;

        var pageMap = new Dictionary<string, Guid>();

        foreach (var pageDef in template.Pages.Where(p => p.ParentSlug == null))
        {
            var page = new Page
            {
                Id = Guid.NewGuid(),
                TenantId = tenantId,
                Title = pageDef.Title,
                Slug = pageDef.Slug,
                PageType = pageDef.PageType,
                Content = pageDef.DefaultContent,
                MenuOrder = pageDef.MenuOrder,
                IsPublished = true,
                ShowInMenu = true
            };

            _context.Pages.Add(page);
            pageMap[pageDef.Slug] = page.Id;

            foreach (var widgetDef in pageDef.Widgets)
            {
                var content = new PageContent
                {
                    PageId = page.Id,
                    ContentType = widgetDef.Type,
                    Title = widgetDef.Title,
                    DisplayOrder = widgetDef.DisplayOrder,
                    IsActive = true
                };
                _context.PageContents.Add(content);
            }
        }

        await _context.SaveChangesAsync();

        foreach (var pageDef in template.Pages.Where(p => p.ParentSlug != null))
        {
            if (!pageMap.ContainsKey(pageDef.ParentSlug!)) continue;

            var page = new Page
            {
                Id = Guid.NewGuid(),
                TenantId = tenantId,
                Title = pageDef.Title,
                Slug = pageDef.Slug,
                PageType = pageDef.PageType,
                Content = pageDef.DefaultContent,
                MenuOrder = pageDef.MenuOrder,
                ParentPageId = pageMap[pageDef.ParentSlug!],
                IsPublished = true,
                ShowInMenu = true
            };

            _context.Pages.Add(page);

            foreach (var widgetDef in pageDef.Widgets)
            {
                var content = new PageContent
                {
                    PageId = page.Id,
                    ContentType = widgetDef.Type,
                    Title = widgetDef.Title,
                    DisplayOrder = widgetDef.DisplayOrder,
                    IsActive = true
                };
                _context.PageContents.Add(content);
            }
        }

        await _context.SaveChangesAsync();
    }
}
