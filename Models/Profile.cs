namespace PortfolioApi.Models;

// Single-row table holding the owner's personal/professional summary.
public class Profile
{
    public int Id { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;          // e.g. "Full-Stack Developer"
    public string Bio { get; set; } = string.Empty;
    public string ShortSummary { get; set; } = string.Empty;   // hero tagline
    public string? ProfileImageUrl { get; set; }
    public string? ResumeUrl { get; set; }
    public string Email { get; set; } = string.Empty;
    public string? Phone { get; set; }
    public string? Location { get; set; }
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}
