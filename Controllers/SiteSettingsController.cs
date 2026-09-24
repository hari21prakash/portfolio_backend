using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PortfolioApi.Common;
using PortfolioApi.Data;
using PortfolioApi.DTOs;

namespace PortfolioApi.Controllers;

[ApiController]
[Route("api/site-settings")]
public class SiteSettingsController : ControllerBase
{
    private readonly ApplicationDbContext _db;

    public SiteSettingsController(ApplicationDbContext db)
    {
        _db = db;
    }

    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> Get()
    {
        var settings = await _db.SiteSettings.FirstOrDefaultAsync();
        if (settings is null) return NotFound(ApiResponse<object>.Fail("Site settings not configured yet."));
        return Ok(ApiResponse<SiteSettingsDto>.Ok(settings.ToDto()));
    }

    [HttpPut]
    [Authorize]
    public async Task<IActionResult> Update([FromBody] SiteSettingsUpdateDto dto)
    {
        var settings = await _db.SiteSettings.FirstOrDefaultAsync();
        if (settings is null)
        {
            settings = new Models.SiteSetting();
            _db.SiteSettings.Add(settings);
        }

        settings.Apply(dto);
        await _db.SaveChangesAsync();
        return Ok(ApiResponse<SiteSettingsDto>.Ok(settings.ToDto(), "Site settings updated."));
    }
}
