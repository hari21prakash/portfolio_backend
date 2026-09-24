using System.ComponentModel.DataAnnotations;
using PortfolioApi.Models;

namespace PortfolioApi.DTOs;

public class ContactMessageDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string? Subject { get; set; }
    public string Message { get; set; } = string.Empty;
    public bool IsRead { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class ContactMessageCreateDto
{
    [Required, MaxLength(150)]
    public string Name { get; set; } = string.Empty;

    [Required, EmailAddress, MaxLength(255)]
    public string Email { get; set; } = string.Empty;

    [MaxLength(200)]
    public string? Subject { get; set; }

    [Required, MaxLength(4000)]
    public string Message { get; set; } = string.Empty;
}

public static class ContactMapping
{
    public static ContactMessageDto ToDto(this ContactMessage c) => new()
    {
        Id = c.Id,
        Name = c.Name,
        Email = c.Email,
        Subject = c.Subject,
        Message = c.Message,
        IsRead = c.IsRead,
        CreatedAt = c.CreatedAt
    };

    public static ContactMessage ToEntity(this ContactMessageCreateDto dto) => new()
    {
        Name = dto.Name,
        Email = dto.Email,
        Subject = dto.Subject,
        Message = dto.Message,
        CreatedAt = DateTime.UtcNow
    };
}
