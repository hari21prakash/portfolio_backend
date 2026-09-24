namespace PortfolioApi.Models;

public class Experience
{
    public int Id { get; set; }
    public string Company { get; set; } = string.Empty;
    public string Position { get; set; } = string.Empty;
    public string? Location { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; }         // null => "Present"
    public string Description { get; set; } = string.Empty;
    public string? TechnologiesUsed { get; set; }  // simple comma-separated text, kept lightweight on purpose
    public int DisplayOrder { get; set; }
    public bool IsActive { get; set; } = true;
}
