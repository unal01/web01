using CoreBuilder.Data;
using CoreBuilder.Models;
using HotChocolate;
using System;
using System.Threading.Tasks;

namespace CoreBuilder.GraphQL
{
    public class Mutation
    {
        public async Task<Tenant> CreateTenant([Service] AppDbContext context, string name, string domain, string category, Guid themeId)
        {
            var tenant = new Tenant
            {
                Name = name,
                Domain = domain,
                Category = category,
                ThemeId = themeId
            };

            context.Tenants.Add(tenant);
            await context.SaveChangesAsync();

            return tenant;
        }

        public async Task<Theme> CreateTheme([Service] AppDbContext context, string name, string primaryColor, string secondaryColor, string fontFamily)
        {
            var theme = new Theme
            {
                Name = name,
                PrimaryColor = primaryColor,
                SecondaryColor = secondaryColor,
                FontFamily = fontFamily
            };

            context.Themes.Add(theme);
            await context.SaveChangesAsync();

            return theme;
        }

        public async Task<Page> CreatePage([Service] AppDbContext context, Guid tenantId, string title, string slug, string content)
        {
            var page = new Page
            {
                TenantId = tenantId,
                Title = title,
                Slug = slug,
                Content = content,
                IsPublished = true
            };

            context.Pages.Add(page);
            await context.SaveChangesAsync();

            return page;
        }

        public async Task<bool> DeleteTenant([Service] AppDbContext context, Guid id)
        {
            var tenant = await context.Tenants.FindAsync(id);
            if (tenant == null) return false;

            context.Tenants.Remove(tenant);
            await context.SaveChangesAsync();

            return true;
        }
    }
}
