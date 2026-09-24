using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PortfolioApi.Common;
using PortfolioApi.Data;
using PortfolioApi.DTOs;
using PortfolioApi.Models;

namespace PortfolioApi.Controllers;

[ApiController]
[Route("api/projects")]
public class ProjectsController : ControllerBase
{
    private readonly ApplicationDbContext _db;

    public ProjectsController(ApplicationDbContext db)
    {
        _db = db;
    }

    // GET /api/projects?featured=true&technology=react&includeInactive=true
    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> GetAll([FromQuery] bool? featured, [FromQuery] string? technology, [FromQuery] bool includeInactive = false)
    {
        var query = _db.Projects
            .Include(p => p.ProjectTechnologies).ThenInclude(pt => pt.Technology)
            .AsQueryable();

        if (!includeInactive)
            query = query.Where(p => p.IsActive);

        if (featured.HasValue)
            query = query.Where(p => p.Featured == featured.Value);

        if (!string.IsNullOrWhiteSpace(technology))
            query = query.Where(p => p.ProjectTechnologies.Any(pt => pt.Technology!.Name == technology));

        var projects = await query.OrderBy(p => p.DisplayOrder).ToListAsync();
        return Ok(ApiResponse<List<ProjectListDto>>.Ok(projects.Select(p => p.ToListDto()).ToList()));
    }

    // GET /api/projects/{idOrSlug} — accepts numeric id (admin edit forms) or slug (public detail page)
    [HttpGet("{idOrSlug}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetOne(string idOrSlug)
    {
        var query = _db.Projects
            .Include(p => p.ProjectTechnologies).ThenInclude(pt => pt.Technology)
            .AsQueryable();

        var project = int.TryParse(idOrSlug, out var id)
            ? await query.FirstOrDefaultAsync(p => p.Id == id)
            : await query.FirstOrDefaultAsync(p => p.Slug == idOrSlug);

        if (project is null) return NotFound(ApiResponse<object>.Fail("Project not found."));
        return Ok(ApiResponse<ProjectDetailDto>.Ok(project.ToDetailDto()));
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> Create([FromBody] ProjectUpsertDto dto)
    {
        var slug = await ResolveUniqueSlugAsync(dto.Slug, dto.Title, null);

        var entity = new Project { CreatedAt = DateTime.UtcNow };
        entity.Apply(dto, slug);

        _db.Projects.Add(entity);
        await _db.SaveChangesAsync(); // need the generated Id before linking technologies

        await SetTechnologiesAsync(entity, dto.TechnologyIds);
        await _db.SaveChangesAsync();

        var created = await _db.Projects
            .Include(p => p.ProjectTechnologies).ThenInclude(pt => pt.Technology)
            .FirstAsync(p => p.Id == entity.Id);

        return Ok(ApiResponse<ProjectDetailDto>.Ok(created.ToDetailDto(), "Project created."));
    }

    [HttpPut("{id:int}")]
    [Authorize]
    public async Task<IActionResult> Update(int id, [FromBody] ProjectUpsertDto dto)
    {
        var entity = await _db.Projects
            .Include(p => p.ProjectTechnologies)
            .FirstOrDefaultAsync(p => p.Id == id);

        if (entity is null) return NotFound(ApiResponse<object>.Fail("Project not found."));

        var slug = await ResolveUniqueSlugAsync(dto.Slug, dto.Title, id);
        entity.Apply(dto, slug);
        await SetTechnologiesAsync(entity, dto.TechnologyIds);
        await _db.SaveChangesAsync();

        var updated = await _db.Projects
            .Include(p => p.ProjectTechnologies).ThenInclude(pt => pt.Technology)
            .FirstAsync(p => p.Id == id);

        return Ok(ApiResponse<ProjectDetailDto>.Ok(updated.ToDetailDto(), "Project updated."));
    }

    [HttpDelete("{id:int}")]
    [Authorize]
    public async Task<IActionResult> Delete(int id)
    {
        var entity = await _db.Projects.FindAsync(id);
        if (entity is null) return NotFound(ApiResponse<object>.Fail("Project not found."));

        _db.Projects.Remove(entity);
        await _db.SaveChangesAsync();
        return Ok(ApiResponse<object>.Ok(new { }, "Project deleted."));
    }

    private async Task<string> ResolveUniqueSlugAsync(string? requestedSlug, string title, int? excludingId)
    {
        var baseSlug = SlugHelper.GenerateSlug(string.IsNullOrWhiteSpace(requestedSlug) ? title : requestedSlug);
        if (string.IsNullOrWhiteSpace(baseSlug)) baseSlug = "project";

        var slug = baseSlug;
        var counter = 2;
        while (await _db.Projects.AnyAsync(p => p.Slug == slug && p.Id != excludingId))
        {
            slug = $"{baseSlug}-{counter}";
            counter++;
        }
        return slug;
    }

    private async Task SetTechnologiesAsync(Project project, List<int> technologyIds)
    {
        // Reload existing links from the DB context tracking, then reconcile.
        var existing = await _db.ProjectTechnologies.Where(pt => pt.ProjectId == project.Id).ToListAsync();
        _db.ProjectTechnologies.RemoveRange(existing);

        foreach (var techId in technologyIds.Distinct())
        {
            _db.ProjectTechnologies.Add(new ProjectTechnology { ProjectId = project.Id, TechnologyId = techId });
        }
    }
}
