using PortfolioApi.Models;

namespace PortfolioApi.DTOs;

public class SiteSettingsDto
{
    public int Id { get; set; }
    public string SiteTitle { get; set; } = string.Empty;
    public string? SiteDescription { get; set; }
    public string? SeoTitle { get; set; }
    public string? SeoDescription { get; set; }
    public string? FaviconUrl { get; set; }
    public string DefaultTheme { get; set; } = "light";
}

public class SiteSettingsUpdateDto
{
    public string SiteTitle { get; set; } = string.Empty;
    public string? SiteDescription { get; set; }
    public string? SeoTitle { get; set; }
    public string? SeoDescription { get; set; }
    public string? FaviconUrl { get; set; }
    public string DefaultTheme { get; set; } = "light";
}

public static class SiteSettingsMapping
{
    public static SiteSettingsDto ToDto(this SiteSetting s) => new()
    {
        Id = s.Id,
        SiteTitle = s.SiteTitle,
        SiteDescription = s.SiteDescription,
        SeoTitle = s.SeoTitle,
        SeoDescription = s.SeoDescription,
        FaviconUrl = s.FaviconUrl,
        DefaultTheme = s.DefaultTheme
    };

    public static void Apply(this SiteSetting s, SiteSettingsUpdateDto dto)
    {
        s.SiteTitle = dto.SiteTitle;
        s.SiteDescription = dto.SiteDescription;
        s.SeoTitle = dto.SeoTitle;
        s.SeoDescription = dto.SeoDescription;
        s.FaviconUrl = dto.FaviconUrl;
        s.DefaultTheme = dto.DefaultTheme;
        s.UpdatedAt = DateTime.UtcNow;
    }
}
