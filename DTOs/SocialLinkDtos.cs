using PortfolioApi.Models;

namespace PortfolioApi.DTOs;

public class SocialLinkDto
{
    public int Id { get; set; }
    public string Platform { get; set; } = string.Empty;
    public string Url { get; set; } = string.Empty;
    public string? Icon { get; set; }
    public int DisplayOrder { get; set; }
    public bool IsActive { get; set; }
}

public class SocialLinkUpsertDto
{
    public string Platform { get; set; } = string.Empty;
    public string Url { get; set; } = string.Empty;
    public string? Icon { get; set; }
    public int DisplayOrder { get; set; }
    public bool IsActive { get; set; } = true;
}

public static class SocialLinkMapping
{
    public static SocialLinkDto ToDto(this SocialLink s) => new()
    {
        Id = s.Id,
        Platform = s.Platform,
        Url = s.Url,
        Icon = s.Icon,
        DisplayOrder = s.DisplayOrder,
        IsActive = s.IsActive
    };

    public static void Apply(this SocialLink s, SocialLinkUpsertDto dto)
    {
        s.Platform = dto.Platform;
        s.Url = dto.Url;
        s.Icon = dto.Icon;
        s.DisplayOrder = dto.DisplayOrder;
        s.IsActive = dto.IsActive;
    }
}
