using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PortfolioApi.Common;
using PortfolioApi.DTOs;
using PortfolioApi.Models;
using PortfolioApi.Services.Generic;

namespace PortfolioApi.Controllers;

[ApiController]
[Route("api/skills")]
public class SkillsController : ControllerBase
{
    private readonly CrudService<Skill> _skills;
    private readonly CrudService<SkillCategory> _categories;

    public SkillsController(CrudService<Skill> skills, CrudService<SkillCategory> categories)
    {
        _skills = skills;
        _categories = categories;
    }

    // GET /api/skills -> categories with their active skills nested, ordered for display
    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> GetAll()
    {
        var categories = await _categories.Query()
            .Include(c => c.Skills.Where(s => s.IsActive))
            .OrderBy(c => c.DisplayOrder)
            .ToListAsync();

        return Ok(ApiResponse<List<SkillCategoryDto>>.Ok(categories.Select(c => c.ToDto()).ToList()));
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> Create([FromBody] SkillUpsertDto dto)
    {
        var entity = new Skill();
        entity.Apply(dto);
        await _skills.AddAsync(entity);
        return Ok(ApiResponse<object>.Ok(new { entity.Id }, "Skill created."));
    }

    [HttpPut("{id:int}")]
    [Authorize]
    public async Task<IActionResult> Update(int id, [FromBody] SkillUpsertDto dto)
    {
        var entity = await _skills.FindAsync(id);
        if (entity is null) return NotFound(ApiResponse<object>.Fail("Skill not found."));

        entity.Apply(dto);
        await _skills.SaveAsync();
        return Ok(ApiResponse<object>.Ok(new { }, "Skill updated."));
    }

    [HttpDelete("{id:int}")]
    [Authorize]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await _skills.DeleteAsync(id);
        if (!deleted) return NotFound(ApiResponse<object>.Fail("Skill not found."));
        return Ok(ApiResponse<object>.Ok(new { }, "Skill deleted."));
    }

    // ---------- Categories ----------

    [HttpGet("categories")]
    [Authorize]
    public async Task<IActionResult> GetCategories()
    {
        var categories = await _categories.Query().OrderBy(c => c.DisplayOrder).ToListAsync();
        return Ok(ApiResponse<List<SkillCategoryDto>>.Ok(categories.Select(c => c.ToDto()).ToList()));
    }

    [HttpPost("categories")]
    [Authorize]
    public async Task<IActionResult> CreateCategory([FromBody] SkillCategoryUpsertDto dto)
    {
        var entity = new SkillCategory();
        entity.Apply(dto);
        await _categories.AddAsync(entity);
        return Ok(ApiResponse<object>.Ok(new { entity.Id }, "Skill category created."));
    }

    [HttpPut("categories/{id:int}")]
    [Authorize]
    public async Task<IActionResult> UpdateCategory(int id, [FromBody] SkillCategoryUpsertDto dto)
    {
        var entity = await _categories.FindAsync(id);
        if (entity is null) return NotFound(ApiResponse<object>.Fail("Category not found."));

        entity.Apply(dto);
        await _categories.SaveAsync();
        return Ok(ApiResponse<object>.Ok(new { }, "Skill category updated."));
    }

    [HttpDelete("categories/{id:int}")]
    [Authorize]
    public async Task<IActionResult> DeleteCategory(int id)
    {
        var deleted = await _categories.DeleteAsync(id);
        if (!deleted) return NotFound(ApiResponse<object>.Fail("Category not found."));
        return Ok(ApiResponse<object>.Ok(new { }, "Skill category deleted."));
    }
}
