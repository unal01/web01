using CoreBuilder.Data;
using CoreBuilder.Models;
using HotChocolate;
using HotChocolate.Types;
using System;
using System.Linq;

namespace CoreBuilder.GraphQL
{
    public class Query
    {
        [UseProjection]
        [UseFiltering]
        [UseSorting]
        public IQueryable<Tenant> GetTenants([Service] AppDbContext context) 
            => context.Tenants;

        [UseProjection]
        public Tenant? GetTenant([Service] AppDbContext context, Guid id) 
            => context.Tenants.FirstOrDefault(t => t.Id == id);

        [UseProjection]
        [UseFiltering]
        [UseSorting]
        public IQueryable<Theme> GetThemes([Service] AppDbContext context) 
            => context.Themes;

        [UseProjection]
        [UseFiltering]
        [UseSorting]
        public IQueryable<Page> GetPages([Service] AppDbContext context) 
            => context.Pages;

        [UseProjection]
        public IQueryable<Page> GetPagesByTenant([Service] AppDbContext context, Guid tenantId) 
            => context.Pages.Where(p => p.TenantId == tenantId);
    }
}
