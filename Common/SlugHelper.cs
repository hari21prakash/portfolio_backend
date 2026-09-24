using System.Text.RegularExpressions;

namespace PortfolioApi.Common;

public static class SlugHelper
{
    public static string GenerateSlug(string input)
    {
        var slug = input.ToLowerInvariant().Trim();
        slug = Regex.Replace(slug, @"[^a-z0-9\s-]", "");
        slug = Regex.Replace(slug, @"\s+", "-");
        slug = Regex.Replace(slug, @"-+", "-");
        return slug.Trim('-');
    }
}
