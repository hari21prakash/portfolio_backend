using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PortfolioApi.Common;
using PortfolioApi.DTOs;
using PortfolioApi.Models;
using PortfolioApi.Services.Generic;

namespace PortfolioApi.Controllers;

[ApiController]
[Route("api/technologies")]
public class TechnologiesController : ControllerBase
{
    private readonly CrudService<Technology> _service;

    public TechnologiesController(CrudService<Technology> service)
    {
        _service = service;
    }

    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> GetAll()
    {
        var items = await _service.Query().Where(t => t.IsActive).OrderBy(t => t.Name).ToListAsync();
        return Ok(ApiResponse<List<TechnologyDto>>.Ok(items.Select(t => t.ToDto()).ToList()));
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> Create([FromBody] TechnologyUpsertDto dto)
    {
        var entity = new Technology();
        entity.Apply(dto);
        await _service.AddAsync(entity);
        return CreatedAtAction(nameof(GetAll), null, ApiResponse<TechnologyDto>.Ok(entity.ToDto(), "Technology created."));
    }

    [HttpPut("{id:int}")]
    [Authorize]
    public async Task<IActionResult> Update(int id, [FromBody] TechnologyUpsertDto dto)
    {
        var entity = await _service.FindAsync(id);
        if (entity is null) return NotFound(ApiResponse<object>.Fail("Technology not found."));

        entity.Apply(dto);
        await _service.SaveAsync();
        return Ok(ApiResponse<TechnologyDto>.Ok(entity.ToDto(), "Technology updated."));
    }

    [HttpDelete("{id:int}")]
    [Authorize]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await _service.DeleteAsync(id);
        if (!deleted) return NotFound(ApiResponse<object>.Fail("Technology not found."));
        return Ok(ApiResponse<object>.Ok(new { }, "Technology deleted."));
    }
}
