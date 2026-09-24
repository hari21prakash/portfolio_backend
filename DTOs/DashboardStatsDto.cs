namespace PortfolioApi.DTOs;

public class DashboardStatsDto
{
    public int TotalProjects { get; set; }
    public int TotalSkills { get; set; }
    public int TotalExperience { get; set; }
    public int TotalCertifications { get; set; }
    public int TotalMessages { get; set; }
    public int UnreadMessages { get; set; }
    public List<ContactMessageDto> RecentMessages { get; set; } = new();
}
