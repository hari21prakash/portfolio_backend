using PortfolioApi.Models;

namespace PortfolioApi.DTOs;

public class ProjectListDto
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public string ShortDescription { get; set; } = string.Empty;
    public string? ImageUrl { get; set; }
    public bool Featured { get; set; }
    public bool IsActive { get; set; }
    public List<TechnologyDto> Technologies { get; set; } = new();
}

public class ProjectDetailDto : ProjectListDto
{
    public string Description { get; set; } = string.Empty;
    public string? GithubUrl { get; set; }
    public string? LiveUrl { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public int DisplayOrder { get; set; }
}

public class ProjectUpsertDto
{
    public string Title { get; set; } = string.Empty;
    public string? Slug { get; set; }   // optional — auto-generated from Title if blank
    public string ShortDescription { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string? ImageUrl { get; set; }
    public string? GithubUrl { get; set; }
    public string? LiveUrl { get; set; }
    public bool Featured { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public int DisplayOrder { get; set; }
    public bool IsActive { get; set; } = true;
    public List<int> TechnologyIds { get; set; } = new();
}

public static class ProjectMapping
{
    public static ProjectListDto ToListDto(this Project p) => new()
    {
        Id = p.Id,
        Title = p.Title,
        Slug = p.Slug,
        ShortDescription = p.ShortDescription,
        ImageUrl = p.ImageUrl,
        Featured = p.Featured,
        IsActive = p.IsActive,
        Technologies = p.ProjectTechnologies?
            .Where(pt => pt.Technology is not null)
            .Select(pt => pt.Technology!.ToDto())
            .ToList() ?? new()
    };

    public static ProjectDetailDto ToDetailDto(this Project p) => new()
    {
        Id = p.Id,
        Title = p.Title,
        Slug = p.Slug,
        ShortDescription = p.ShortDescription,
        ImageUrl = p.ImageUrl,
        Featured = p.Featured,
        IsActive = p.IsActive,
        Technologies = p.ProjectTechnologies?
            .Where(pt => pt.Technology is not null)
            .Select(pt => pt.Technology!.ToDto())
            .ToList() ?? new(),
        Description = p.Description,
        GithubUrl = p.GithubUrl,
        LiveUrl = p.LiveUrl,
        StartDate = p.StartDate,
        EndDate = p.EndDate,
        DisplayOrder = p.DisplayOrder
    };

    public static void Apply(this Project p, ProjectUpsertDto dto, string slug)
    {
        p.Title = dto.Title;
        p.Slug = slug;
        p.ShortDescription = dto.ShortDescription;
        p.Description = dto.Description;
        p.ImageUrl = dto.ImageUrl;
        p.GithubUrl = dto.GithubUrl;
        p.LiveUrl = dto.LiveUrl;
        p.Featured = dto.Featured;
        p.StartDate = dto.StartDate;
        p.EndDate = dto.EndDate;
        p.DisplayOrder = dto.DisplayOrder;
        p.IsActive = dto.IsActive;
        p.UpdatedAt = DateTime.UtcNow;
    }
}
