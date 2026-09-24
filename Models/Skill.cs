namespace PortfolioApi.Models;

public class Skill
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Icon { get; set; }
    public int Level { get; set; }          // 0-100 proficiency, optional use in UI
    public int DisplayOrder { get; set; }
    public bool IsActive { get; set; } = true;

    public int SkillCategoryId { get; set; }
    public SkillCategory? SkillCategory { get; set; }
}
