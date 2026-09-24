using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PortfolioApi.Common;
using PortfolioApi.DTOs;
using PortfolioApi.Models;
using PortfolioApi.Services.Generic;

namespace PortfolioApi.Controllers;

[ApiController]
[Route("api/certifications")]
public class CertificationsController : ControllerBase
{
    private readonly CrudService<Certification> _service;

    public CertificationsController(CrudService<Certification> service)
    {
        _service = service;
    }

    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> GetAll()
    {
        var items = await _service.Query().Where(c => c.IsActive)
            .OrderBy(c => c.DisplayOrder).ThenByDescending(c => c.IssueDate).ToListAsync();
        return Ok(ApiResponse<List<CertificationDto>>.Ok(items.Select(c => c.ToDto()).ToList()));
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> Create([FromBody] CertificationUpsertDto dto)
    {
        var entity = new Certification();
        entity.Apply(dto);
        await _service.AddAsync(entity);
        return Ok(ApiResponse<CertificationDto>.Ok(entity.ToDto(), "Certification created."));
    }

    [HttpPut("{id:int}")]
    [Authorize]
    public async Task<IActionResult> Update(int id, [FromBody] CertificationUpsertDto dto)
    {
        var entity = await _service.FindAsync(id);
        if (entity is null) return NotFound(ApiResponse<object>.Fail("Certification not found."));

        entity.Apply(dto);
        await _service.SaveAsync();
        return Ok(ApiResponse<CertificationDto>.Ok(entity.ToDto(), "Certification updated."));
    }

    [HttpDelete("{id:int}")]
    [Authorize]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await _service.DeleteAsync(id);
        if (!deleted) return NotFound(ApiResponse<object>.Fail("Certification not found."));
        return Ok(ApiResponse<object>.Ok(new { }, "Certification deleted."));
    }
}
