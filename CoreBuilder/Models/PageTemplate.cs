using System;
using System.Collections.Generic;

namespace CoreBuilder.Models;

/// <summary>
/// Sayfa ?ablonlar? - yeni sayfa olu?tururken kullan?lacak
/// </summary>
public class PageTemplate
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public PageType PageType { get; set; }
    public string Icon { get; set; } = "oi-document";
    public List<ContentType> DefaultWidgets { get; set; } = new();
    public string DefaultContent { get; set; } = string.Empty;
    public bool IsParentCapable { get; set; } = true; // Üst sayfa olabilir mi?
}

/// <summary>
/// Site ?ablonlar? - yeni site olu?tururken seçilecek
/// </summary>
public class SiteTemplate
{
    public string Id { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public string Icon { get; set; } = "oi-globe";
    public List<PageTemplateDefinition> Pages { get; set; } = new();
}

/// <summary>
/// ?ablonda tan?ml? sayfa yap?land?rmas?
/// </summary>
public class PageTemplateDefinition
{
    public string Title { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public PageType PageType { get; set; }
    public int MenuOrder { get; set; }
    public string? ParentSlug { get; set; } // Üst sayfa slug'?
    public List<WidgetDefinition> Widgets { get; set; } = new();
    public string DefaultContent { get; set; } = string.Empty;
}

/// <summary>
/// Widget tan?m?
/// </summary>
public class WidgetDefinition
{
    public ContentType Type { get; set; }
    public string Title { get; set; } = string.Empty;
    public int DisplayOrder { get; set; }
    public Dictionary<string, string> Settings { get; set; } = new();
}
