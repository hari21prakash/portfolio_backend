using PortfolioApi.Models;

namespace PortfolioApi.DTOs;

public class ExperienceDto
{
    public int Id { get; set; }
    public string Company { get; set; } = string.Empty;
    public string Position { get; set; } = string.Empty;
    public string? Location { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public string Description { get; set; } = string.Empty;
    public string? TechnologiesUsed { get; set; }
    public int DisplayOrder { get; set; }
    public bool IsActive { get; set; }
}

public class ExperienceUpsertDto
{
    public string Company { get; set; } = string.Empty;
    public string Position { get; set; } = string.Empty;
    public string? Location { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public string Description { get; set; } = string.Empty;
    public string? TechnologiesUsed { get; set; }
    public int DisplayOrder { get; set; }
    public bool IsActive { get; set; } = true;
}

public static class ExperienceMapping
{
    public static ExperienceDto ToDto(this Experience e) => new()
    {
        Id = e.Id,
        Company = e.Company,
        Position = e.Position,
        Location = e.Location,
        StartDate = e.StartDate,
        EndDate = e.EndDate,
        Description = e.Description,
        TechnologiesUsed = e.TechnologiesUsed,
        DisplayOrder = e.DisplayOrder,
        IsActive = e.IsActive
    };

    public static void Apply(this Experience e, ExperienceUpsertDto dto)
    {
        e.Company = dto.Company;
        e.Position = dto.Position;
        e.Location = dto.Location;
        e.StartDate = dto.StartDate;
        e.EndDate = dto.EndDate;
        e.Description = dto.Description;
        e.TechnologiesUsed = dto.TechnologiesUsed;
        e.DisplayOrder = dto.DisplayOrder;
        e.IsActive = dto.IsActive;
    }
}
