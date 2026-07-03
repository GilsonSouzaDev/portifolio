using Portfolio.API.Models;

namespace Portfolio.API.DTOs;

public class SkillDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public SkillCategory Category { get; set; }
    public string Description { get; set; } = string.Empty;
    public int ProficiencyLevel { get; set; }
    public string? IconUrl { get; set; }
    public int DisplayOrder { get; set; }
}

public class CreateOrUpdateSkillRequest
{
    public string Name { get; set; } = string.Empty;
    public SkillCategory Category { get; set; }
    public string Description { get; set; } = string.Empty;
    public int ProficiencyLevel { get; set; }
    public string? IconUrl { get; set; }
    public int DisplayOrder { get; set; }
}
