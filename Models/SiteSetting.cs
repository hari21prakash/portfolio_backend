namespace PortfolioApi.Models;

// Single-row table for global site/SEO configuration.
public class SiteSetting
{
    public int Id { get; set; }
    public string SiteTitle { get; set; } = string.Empty;
    public string? SiteDescription { get; set; }
    public string? SeoTitle { get; set; }
    public string? SeoDescription { get; set; }
    public string? FaviconUrl { get; set; }
    public string DefaultTheme { get; set; } = "light";    // "light" | "dark"
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}
