using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PortfolioApi.Common;
using PortfolioApi.DTOs;
using PortfolioApi.Models;
using PortfolioApi.Services.Generic;

namespace PortfolioApi.Controllers;

[ApiController]
[Route("api/social-links")]
public class SocialLinksController : ControllerBase
{
    private readonly CrudService<SocialLink> _service;

    public SocialLinksController(CrudService<SocialLink> service)
    {
        _service = service;
    }

    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> GetAll()
    {
        var items = await _service.Query().Where(s => s.IsActive)
            .OrderBy(s => s.DisplayOrder).ToListAsync();
        return Ok(ApiResponse<List<SocialLinkDto>>.Ok(items.Select(s => s.ToDto()).ToList()));
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> Create([FromBody] SocialLinkUpsertDto dto)
    {
        var entity = new SocialLink();
        entity.Apply(dto);
        await _service.AddAsync(entity);
        return Ok(ApiResponse<SocialLinkDto>.Ok(entity.ToDto(), "Social link created."));
    }

    [HttpPut("{id:int}")]
    [Authorize]
    public async Task<IActionResult> Update(int id, [FromBody] SocialLinkUpsertDto dto)
    {
        var entity = await _service.FindAsync(id);
        if (entity is null) return NotFound(ApiResponse<object>.Fail("Social link not found."));

        entity.Apply(dto);
        await _service.SaveAsync();
        return Ok(ApiResponse<SocialLinkDto>.Ok(entity.ToDto(), "Social link updated."));
    }

    [HttpDelete("{id:int}")]
    [Authorize]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await _service.DeleteAsync(id);
        if (!deleted) return NotFound(ApiResponse<object>.Fail("Social link not found."));
        return Ok(ApiResponse<object>.Ok(new { }, "Social link deleted."));
    }
}
