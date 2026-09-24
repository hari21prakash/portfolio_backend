using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PortfolioApi.Common;
using PortfolioApi.DTOs;
using PortfolioApi.Models;
using PortfolioApi.Services.Generic;

namespace PortfolioApi.Controllers;

[ApiController]
[Route("api/experience")]
public class ExperienceController : ControllerBase
{
    private readonly CrudService<Experience> _service;

    public ExperienceController(CrudService<Experience> service)
    {
        _service = service;
    }

    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> GetAll()
    {
        var items = await _service.Query().Where(e => e.IsActive)
            .OrderBy(e => e.DisplayOrder).ThenByDescending(e => e.StartDate).ToListAsync();
        return Ok(ApiResponse<List<ExperienceDto>>.Ok(items.Select(e => e.ToDto()).ToList()));
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> Create([FromBody] ExperienceUpsertDto dto)
    {
        var entity = new Experience();
        entity.Apply(dto);
        await _service.AddAsync(entity);
        return Ok(ApiResponse<ExperienceDto>.Ok(entity.ToDto(), "Experience created."));
    }

    [HttpPut("{id:int}")]
    [Authorize]
    public async Task<IActionResult> Update(int id, [FromBody] ExperienceUpsertDto dto)
    {
        var entity = await _service.FindAsync(id);
        if (entity is null) return NotFound(ApiResponse<object>.Fail("Experience not found."));

        entity.Apply(dto);
        await _service.SaveAsync();
        return Ok(ApiResponse<ExperienceDto>.Ok(entity.ToDto(), "Experience updated."));
    }

    [HttpDelete("{id:int}")]
    [Authorize]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await _service.DeleteAsync(id);
        if (!deleted) return NotFound(ApiResponse<object>.Fail("Experience not found."));
        return Ok(ApiResponse<object>.Ok(new { }, "Experience deleted."));
    }
}
