using CoreBuilder.Models;
using System.Collections.Generic;
using System;

namespace CoreBuilder.Factories
{
    public interface ISiteTemplateFactory
    {
        List<Page> CreateDefaultPages(Guid tenantId);
        List<Module> CreateDefaultModules(Guid tenantId);
        List<SiteSetting> CreateDefaultSettings(Guid tenantId);
        Theme CreateDefaultTheme(Guid tenantId);
    }
}
