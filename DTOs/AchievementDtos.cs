using PortfolioApi.Models;

namespace PortfolioApi.DTOs;

public class AchievementDto
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DateTime? Date { get; set; }
    public string? ImageUrl { get; set; }
    public int DisplayOrder { get; set; }
    public bool IsActive { get; set; }
}

public class AchievementUpsertDto
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DateTime? Date { get; set; }
    public string? ImageUrl { get; set; }
    public int DisplayOrder { get; set; }
    public bool IsActive { get; set; } = true;
}

public static class AchievementMapping
{
    public static AchievementDto ToDto(this Achievement a) => new()
    {
        Id = a.Id,
        Title = a.Title,
        Description = a.Description,
        Date = a.Date,
        ImageUrl = a.ImageUrl,
        DisplayOrder = a.DisplayOrder,
        IsActive = a.IsActive
    };

    public static void Apply(this Achievement a, AchievementUpsertDto dto)
    {
        a.Title = dto.Title;
        a.Description = dto.Description;
        a.Date = dto.Date;
        a.ImageUrl = dto.ImageUrl;
        a.DisplayOrder = dto.DisplayOrder;
        a.IsActive = dto.IsActive;
    }
}
