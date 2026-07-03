using Microsoft.AspNetCore.Mvc;
using Portfolio.API.DTOs;
using Portfolio.API.Services.Interfaces;

namespace Portfolio.API.Controllers;

[ApiController]
[Route("api/skills")]
public class SkillsController : ControllerBase
{
    private readonly ISkillsService _skillsService;

    public SkillsController(ISkillsService skillsService)
    {
        _skillsService = skillsService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var skills = await _skillsService.GetAllSkillsAsync();
        return Ok(skills);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateOrUpdateSkillRequest skillRequest)
    {
        var skill = await _skillsService.CreateSkillAsync(skillRequest);
        return CreatedAtAction(nameof(GetAll), new { id = skill.Id }, skill);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] CreateOrUpdateSkillRequest skillRequest)
    {
        var skill = await _skillsService.UpdateSkillAsync(id, skillRequest);

        if (skill == null)
            return NotFound(new { message = "Habilidade não encontrada." });

        return Ok(skill);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _skillsService.DeleteSkillAsync(id);

        if (!result)
            return NotFound(new { message = "Habilidade não encontrada." });

        return NoContent();
    }
}
