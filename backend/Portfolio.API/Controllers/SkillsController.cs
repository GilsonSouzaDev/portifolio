using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Portfolio.API.Data;
using Portfolio.API.Models;

namespace Portfolio.API.Controllers;

[ApiController]
[Route("api/skills")]
public class SkillsController : ControllerBase
{
    private readonly AppDbContext _context;

    public SkillsController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var skills = await _context.Skills
            .OrderBy(s => s.DisplayOrder)
            .ToListAsync();
        return Ok(skills);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var skill = await _context.Skills.FindAsync(id);
        if (skill == null)
            return NotFound(new { message = "Skill não encontrada." });
        return Ok(skill);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] Skill skill)
    {
        skill.CreatedAt = DateTime.UtcNow;
        skill.UpdatedAt = DateTime.UtcNow;
        _context.Skills.Add(skill);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetById), new { id = skill.Id }, skill);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] Skill updated)
    {
        var skill = await _context.Skills.FindAsync(id);
        if (skill == null)
            return NotFound(new { message = "Skill não encontrada." });

        skill.Name = updated.Name;
        skill.Category = updated.Category;
        skill.Description = updated.Description;
        skill.ProficiencyLevel = updated.ProficiencyLevel;
        skill.IconUrl = updated.IconUrl;
        skill.DisplayOrder = updated.DisplayOrder;
        skill.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();
        return Ok(skill);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var skill = await _context.Skills.FindAsync(id);
        if (skill == null)
            return NotFound(new { message = "Skill não encontrada." });

        _context.Skills.Remove(skill);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}
