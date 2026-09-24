using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PortfolioApi.Common;
using PortfolioApi.DTOs;
using PortfolioApi.Models;
using PortfolioApi.Services.Generic;

namespace PortfolioApi.Controllers;

[ApiController]
[Route("api/achievements")]
public class AchievementsController : ControllerBase
{
    private readonly CrudService<Achievement> _service;

    public AchievementsController(CrudService<Achievement> service)
    {
        _service = service;
    }

    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> GetAll()
    {
        var items = await _service.Query().Where(a => a.IsActive)
            .OrderBy(a => a.DisplayOrder).ToListAsync();
        return Ok(ApiResponse<List<AchievementDto>>.Ok(items.Select(a => a.ToDto()).ToList()));
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> Create([FromBody] AchievementUpsertDto dto)
    {
        var entity = new Achievement();
        entity.Apply(dto);
        await _service.AddAsync(entity);
        return Ok(ApiResponse<AchievementDto>.Ok(entity.ToDto(), "Achievement created."));
    }

    [HttpPut("{id:int}")]
    [Authorize]
    public async Task<IActionResult> Update(int id, [FromBody] AchievementUpsertDto dto)
    {
        var entity = await _service.FindAsync(id);
        if (entity is null) return NotFound(ApiResponse<object>.Fail("Achievement not found."));

        entity.Apply(dto);
        await _service.SaveAsync();
        return Ok(ApiResponse<AchievementDto>.Ok(entity.ToDto(), "Achievement updated."));
    }

    [HttpDelete("{id:int}")]
    [Authorize]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await _service.DeleteAsync(id);
        if (!deleted) return NotFound(ApiResponse<object>.Fail("Achievement not found."));
        return Ok(ApiResponse<object>.Ok(new { }, "Achievement deleted."));
    }
}
