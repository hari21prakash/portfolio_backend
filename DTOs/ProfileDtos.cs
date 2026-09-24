using PortfolioApi.Models;

namespace PortfolioApi.DTOs;

public class ProfileDto
{
    public int Id { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string Bio { get; set; } = string.Empty;
    public string ShortSummary { get; set; } = string.Empty;
    public string? ProfileImageUrl { get; set; }
    public string? ResumeUrl { get; set; }
    public string Email { get; set; } = string.Empty;
    public string? Phone { get; set; }
    public string? Location { get; set; }
}

public class ProfileUpdateDto
{
    public string FullName { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string Bio { get; set; } = string.Empty;
    public string ShortSummary { get; set; } = string.Empty;
    public string? ProfileImageUrl { get; set; }
    public string? ResumeUrl { get; set; }
    public string Email { get; set; } = string.Empty;
    public string? Phone { get; set; }
    public string? Location { get; set; }
}

public static class ProfileMapping
{
    public static ProfileDto ToDto(this Profile p) => new()
    {
        Id = p.Id,
        FullName = p.FullName,
        Title = p.Title,
        Bio = p.Bio,
        ShortSummary = p.ShortSummary,
        ProfileImageUrl = p.ProfileImageUrl,
        ResumeUrl = p.ResumeUrl,
        Email = p.Email,
        Phone = p.Phone,
        Location = p.Location
    };

    public static void Apply(this Profile p, ProfileUpdateDto dto)
    {
        p.FullName = dto.FullName;
        p.Title = dto.Title;
        p.Bio = dto.Bio;
        p.ShortSummary = dto.ShortSummary;
        p.ProfileImageUrl = dto.ProfileImageUrl;
        p.ResumeUrl = dto.ResumeUrl;
        p.Email = dto.Email;
        p.Phone = dto.Phone;
        p.Location = dto.Location;
        p.UpdatedAt = DateTime.UtcNow;
    }
}
