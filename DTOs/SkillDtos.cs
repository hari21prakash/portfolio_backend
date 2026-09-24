using PortfolioApi.Models;

namespace PortfolioApi.DTOs;

public class SkillDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Icon { get; set; }
    public int Level { get; set; }
    public int DisplayOrder { get; set; }
    public bool IsActive { get; set; }
    public int SkillCategoryId { get; set; }
    public string SkillCategoryName { get; set; } = string.Empty;
}

public class SkillUpsertDto
{
    public string Name { get; set; } = string.Empty;
    public string? Icon { get; set; }
    public int Level { get; set; }
    public int DisplayOrder { get; set; }
    public bool IsActive { get; set; } = true;
    public int SkillCategoryId { get; set; }
}

public class SkillCategoryDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int DisplayOrder { get; set; }
    public List<SkillDto> Skills { get; set; } = new();
}

public class SkillCategoryUpsertDto
{
    public string Name { get; set; } = string.Empty;
    public int DisplayOrder { get; set; }
}

public static class SkillMapping
{
    public static SkillDto ToDto(this Skill s) => new()
    {
        Id = s.Id,
        Name = s.Name,
        Icon = s.Icon,
        Level = s.Level,
        DisplayOrder = s.DisplayOrder,
        IsActive = s.IsActive,
        SkillCategoryId = s.SkillCategoryId,
        SkillCategoryName = s.SkillCategory?.Name ?? string.Empty
    };

    public static void Apply(this Skill s, SkillUpsertDto dto)
    {
        s.Name = dto.Name;
        s.Icon = dto.Icon;
        s.Level = dto.Level;
        s.DisplayOrder = dto.DisplayOrder;
        s.IsActive = dto.IsActive;
        s.SkillCategoryId = dto.SkillCategoryId;
    }

    public static SkillCategoryDto ToDto(this SkillCategory c) => new()
    {
        Id = c.Id,
        Name = c.Name,
        DisplayOrder = c.DisplayOrder,
        Skills = c.Skills?.OrderBy(s => s.DisplayOrder).Select(s => s.ToDto()).ToList() ?? new()
    };

    public static void Apply(this SkillCategory c, SkillCategoryUpsertDto dto)
    {
        c.Name = dto.Name;
        c.DisplayOrder = dto.DisplayOrder;
    }
}
