namespace PortfolioApi.Models;

public class SkillCategory
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;   // e.g. "Frontend", "Backend", "DevOps"
    public int DisplayOrder { get; set; }

    public ICollection<Skill> Skills { get; set; } = new List<Skill>();
}
