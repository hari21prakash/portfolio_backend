using PortfolioApi.Models;

namespace PortfolioApi.DTOs;

public class TechnologyDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Icon { get; set; }
    public string? Category { get; set; }
    public bool IsActive { get; set; }
}

public class TechnologyUpsertDto
{
    public string Name { get; set; } = string.Empty;
    public string? Icon { get; set; }
    public string? Category { get; set; }
    public bool IsActive { get; set; } = true;
}

public static class TechnologyMapping
{
    public static TechnologyDto ToDto(this Technology t) => new()
    {
        Id = t.Id,
        Name = t.Name,
        Icon = t.Icon,
        Category = t.Category,
        IsActive = t.IsActive
    };

    public static void Apply(this Technology t, TechnologyUpsertDto dto)
    {
        t.Name = dto.Name;
        t.Icon = dto.Icon;
        t.Category = dto.Category;
        t.IsActive = dto.IsActive;
    }
}
