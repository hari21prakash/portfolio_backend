namespace PortfolioApi.Models;

// Tech tags reused across projects (and referenced loosely by Experience via text).
public class Technology
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Icon { get; set; }
    public string? Category { get; set; }   // e.g. "Language", "Framework", "Database", "Tool"
    public bool IsActive { get; set; } = true;

    public ICollection<ProjectTechnology> ProjectTechnologies { get; set; } = new List<ProjectTechnology>();
}
