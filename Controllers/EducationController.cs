using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PortfolioApi.Common;
using PortfolioApi.DTOs;
using PortfolioApi.Models;
using PortfolioApi.Services.Generic;

namespace PortfolioApi.Controllers;

[ApiController]
[Route("api/education")]
public class EducationController : ControllerBase
{
    private readonly CrudService<Education> _service;

    public EducationController(CrudService<Education> service)
    {
        _service = service;
    }

    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> GetAll()
    {
        var items = await _service.Query().Where(e => e.IsActive)
            .OrderBy(e => e.DisplayOrder).ThenByDescending(e => e.StartDate).ToListAsync();
        return Ok(ApiResponse<List<EducationDto>>.Ok(items.Select(e => e.ToDto()).ToList()));
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> Create([FromBody] EducationUpsertDto dto)
    {
        var entity = new Education();
        entity.Apply(dto);
        await _service.AddAsync(entity);
        return Ok(ApiResponse<EducationDto>.Ok(entity.ToDto(), "Education created."));
    }

    [HttpPut("{id:int}")]
    [Authorize]
    public async Task<IActionResult> Update(int id, [FromBody] EducationUpsertDto dto)
    {
        var entity = await _service.FindAsync(id);
        if (entity is null) return NotFound(ApiResponse<object>.Fail("Education not found."));

        entity.Apply(dto);
        await _service.SaveAsync();
        return Ok(ApiResponse<EducationDto>.Ok(entity.ToDto(), "Education updated."));
    }

    [HttpDelete("{id:int}")]
    [Authorize]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await _service.DeleteAsync(id);
        if (!deleted) return NotFound(ApiResponse<object>.Fail("Education not found."));
        return Ok(ApiResponse<object>.Ok(new { }, "Education deleted."));
    }
}
