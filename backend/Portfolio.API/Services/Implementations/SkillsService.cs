using Microsoft.EntityFrameworkCore;
using Portfolio.API.Data;
using Portfolio.API.DTOs;
using Portfolio.API.Models;
using Portfolio.API.Services.Interfaces;

namespace Portfolio.API.Services.Implementations;

public class SkillsService : ISkillsService
{
    private readonly AppDbContext _context;

    public SkillsService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<SkillDto>> GetAllSkillsAsync()
    {
        return await _context.Skills
            .OrderBy(s => s.DisplayOrder)
            .Select(s => new SkillDto
            {
                Id = s.Id,
                Name = s.Name,
                Category = s.Category,
                Description = s.Description,
                ProficiencyLevel = s.ProficiencyLevel,
                IconUrl = s.IconUrl,
                DisplayOrder = s.DisplayOrder
            })
            .ToListAsync();
    }

    public async Task<SkillDto> CreateSkillAsync(CreateOrUpdateSkillRequest request)
    {
        var skill = new Skill
        {
            Name = request.Name ?? string.Empty,
            Category = request.Category,
            Description = request.Description ?? string.Empty,
            ProficiencyLevel = request.ProficiencyLevel,
            IconUrl = request.IconUrl,
            DisplayOrder = request.DisplayOrder,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        _context.Skills.Add(skill);
        await _context.SaveChangesAsync();

        return new SkillDto
        {
            Id = skill.Id,
            Name = skill.Name,
            Category = skill.Category,
            Description = skill.Description,
            ProficiencyLevel = skill.ProficiencyLevel,
            IconUrl = skill.IconUrl,
            DisplayOrder = skill.DisplayOrder
        };
    }

    public async Task<SkillDto?> UpdateSkillAsync(int id, CreateOrUpdateSkillRequest request)
    {
        var skill = await _context.Skills.FindAsync(id);
        if (skill == null) return null;

        skill.Name = request.Name ?? string.Empty;
        skill.Category = request.Category;
        skill.Description = request.Description ?? string.Empty;
        skill.ProficiencyLevel = request.ProficiencyLevel;
        skill.IconUrl = request.IconUrl;
        skill.DisplayOrder = request.DisplayOrder;
        skill.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        return new SkillDto
        {
            Id = skill.Id,
            Name = skill.Name,
            Category = skill.Category,
            Description = skill.Description,
            ProficiencyLevel = skill.ProficiencyLevel,
            IconUrl = skill.IconUrl,
            DisplayOrder = skill.DisplayOrder
        };
    }

    public async Task<bool> DeleteSkillAsync(int id)
    {
        var skill = await _context.Skills.FindAsync(id);
        if (skill == null) return false;

        _context.Skills.Remove(skill);
        await _context.SaveChangesAsync();
        return true;
    }
}
