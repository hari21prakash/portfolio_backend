namespace PortfolioApi.Models;

public class SocialLink
{
    public int Id { get; set; }
    public string Platform { get; set; } = string.Empty;  // e.g. "GitHub", "LinkedIn"
    public string Url { get; set; } = string.Empty;
    public string? Icon { get; set; }
    public int DisplayOrder { get; set; }
    public bool IsActive { get; set; } = true;
}
