using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PortfolioApi.Common;
using PortfolioApi.DTOs;
using PortfolioApi.Models;
using PortfolioApi.Services.Generic;

namespace PortfolioApi.Controllers;

[ApiController]
[Route("api/contact")]
public class ContactController : ControllerBase
{
    private readonly CrudService<ContactMessage> _service;

    public ContactController(CrudService<ContactMessage> service)
    {
        _service = service;
    }

    // POST /api/contact — public, anyone can submit the contact form
    [HttpPost]
    [AllowAnonymous]
    public async Task<IActionResult> Submit([FromBody] ContactMessageCreateDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ApiResponse<object>.Fail("Please fill in all required fields correctly."));

        var entity = dto.ToEntity();
        await _service.AddAsync(entity);
        return Ok(ApiResponse<object>.Ok(new { }, "Message sent successfully."));
    }

    // GET /api/contact — admin only, newest first
    [HttpGet]
    [Authorize]
    public async Task<IActionResult> GetAll()
    {
        var items = await _service.Query().OrderByDescending(c => c.CreatedAt).ToListAsync();
        return Ok(ApiResponse<List<ContactMessageDto>>.Ok(items.Select(c => c.ToDto()).ToList()));
    }

    [HttpPut("{id:int}/read")]
    [Authorize]
    public async Task<IActionResult> MarkRead(int id)
    {
        var entity = await _service.FindAsync(id);
        if (entity is null) return NotFound(ApiResponse<object>.Fail("Message not found."));

        entity.IsRead = true;
        await _service.SaveAsync();
        return Ok(ApiResponse<ContactMessageDto>.Ok(entity.ToDto(), "Marked as read."));
    }

    [HttpDelete("{id:int}")]
    [Authorize]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await _service.DeleteAsync(id);
        if (!deleted) return NotFound(ApiResponse<object>.Fail("Message not found."));
        return Ok(ApiResponse<object>.Ok(new { }, "Message deleted."));
    }
}
