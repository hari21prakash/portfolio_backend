using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PortfolioApi.Common;
using PortfolioApi.Data;
using PortfolioApi.DTOs;

namespace PortfolioApi.Controllers;

[ApiController]
[Route("api/dashboard")]
[Authorize]
public class DashboardController : ControllerBase
{
    private readonly ApplicationDbContext _db;

    public DashboardController(ApplicationDbContext db)
    {
        _db = db;
    }

    [HttpGet("stats")]
    public async Task<IActionResult> GetStats()
    {
        var recentMessages = await _db.ContactMessages
            .OrderByDescending(m => m.CreatedAt)
            .Take(5)
            .ToListAsync(); // materialize first — ToDto() can't be translated to SQL

        var stats = new DashboardStatsDto
        {
            TotalProjects = await _db.Projects.CountAsync(),
            TotalSkills = await _db.Skills.CountAsync(),
            TotalExperience = await _db.Experiences.CountAsync(),
            TotalCertifications = await _db.Certifications.CountAsync(),
            TotalMessages = await _db.ContactMessages.CountAsync(),
            UnreadMessages = await _db.ContactMessages.CountAsync(m => !m.IsRead),
            RecentMessages = recentMessages.Select(m => m.ToDto()).ToList()
        };

        return Ok(ApiResponse<DashboardStatsDto>.Ok(stats));
    }
}
