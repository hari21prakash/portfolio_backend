using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PortfolioApi.Common;
using PortfolioApi.DTOs;
using PortfolioApi.Models;
using PortfolioApi.Services.Generic;

namespace PortfolioApi.Controllers;

[ApiController]
[Route("api/services")]
public class ServicesController : ControllerBase
{
    private readonly CrudService<Service> _service;

    public ServicesController(CrudService<Service> service)
    {
        _service = service;
    }

    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> GetAll()
    {
        var items = await _service.Query().Where(s => s.IsActive)
            .OrderBy(s => s.DisplayOrder).ToListAsync();
        return Ok(ApiResponse<List<ServiceDto>>.Ok(items.Select(s => s.ToDto()).ToList()));
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> Create([FromBody] ServiceUpsertDto dto)
    {
        var entity = new Service();
        entity.Apply(dto);
        await _service.AddAsync(entity);
        return Ok(ApiResponse<ServiceDto>.Ok(entity.ToDto(), "Service created."));
    }

    [HttpPut("{id:int}")]
    [Authorize]
    public async Task<IActionResult> Update(int id, [FromBody] ServiceUpsertDto dto)
    {
        var entity = await _service.FindAsync(id);
        if (entity is null) return NotFound(ApiResponse<object>.Fail("Service not found."));

        entity.Apply(dto);
        await _service.SaveAsync();
        return Ok(ApiResponse<ServiceDto>.Ok(entity.ToDto(), "Service updated."));
    }

    [HttpDelete("{id:int}")]
    [Authorize]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await _service.DeleteAsync(id);
        if (!deleted) return NotFound(ApiResponse<object>.Fail("Service not found."));
        return Ok(ApiResponse<object>.Ok(new { }, "Service deleted."));
    }
}
