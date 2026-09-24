using PortfolioApi.Models;

namespace PortfolioApi.DTOs;

public class ServiceDto
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string? Icon { get; set; }
    public int DisplayOrder { get; set; }
    public bool IsActive { get; set; }
}

public class ServiceUpsertDto
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string? Icon { get; set; }
    public int DisplayOrder { get; set; }
    public bool IsActive { get; set; } = true;
}

public static class ServiceMapping
{
    public static ServiceDto ToDto(this Service s) => new()
    {
        Id = s.Id,
        Title = s.Title,
        Description = s.Description,
        Icon = s.Icon,
        DisplayOrder = s.DisplayOrder,
        IsActive = s.IsActive
    };

    public static void Apply(this Service s, ServiceUpsertDto dto)
    {
        s.Title = dto.Title;
        s.Description = dto.Description;
        s.Icon = dto.Icon;
        s.DisplayOrder = dto.DisplayOrder;
        s.IsActive = dto.IsActive;
    }
}
