using PortfolioApi.Models;

namespace PortfolioApi.DTOs;

public class EducationDto
{
    public int Id { get; set; }
    public string Institution { get; set; } = string.Empty;
    public string Degree { get; set; } = string.Empty;
    public string? Field { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public string? PercentageOrCgpa { get; set; }
    public string? Description { get; set; }
    public int DisplayOrder { get; set; }
    public bool IsActive { get; set; }
}

public class EducationUpsertDto
{
    public string Institution { get; set; } = string.Empty;
    public string Degree { get; set; } = string.Empty;
    public string? Field { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public string? PercentageOrCgpa { get; set; }
    public string? Description { get; set; }
    public int DisplayOrder { get; set; }
    public bool IsActive { get; set; } = true;
}

public static class EducationMapping
{
    public static EducationDto ToDto(this Education e) => new()
    {
        Id = e.Id,
        Institution = e.Institution,
        Degree = e.Degree,
        Field = e.Field,
        StartDate = e.StartDate,
        EndDate = e.EndDate,
        PercentageOrCgpa = e.PercentageOrCgpa,
        Description = e.Description,
        DisplayOrder = e.DisplayOrder,
        IsActive = e.IsActive
    };

    public static void Apply(this Education e, EducationUpsertDto dto)
    {
        e.Institution = dto.Institution;
        e.Degree = dto.Degree;
        e.Field = dto.Field;
        e.StartDate = dto.StartDate;
        e.EndDate = dto.EndDate;
        e.PercentageOrCgpa = dto.PercentageOrCgpa;
        e.Description = dto.Description;
        e.DisplayOrder = dto.DisplayOrder;
        e.IsActive = dto.IsActive;
    }
}
