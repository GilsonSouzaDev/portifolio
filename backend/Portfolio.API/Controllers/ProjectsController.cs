using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Portfolio.API.Data;
using Portfolio.API.Models;

namespace Portfolio.API.Controllers;

[ApiController]
[Route("api/projects")]
public class ProjectsController : ControllerBase
{
    private readonly AppDbContext _context;

    public ProjectsController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var projects = await _context.Projects
            .OrderBy(p => p.DisplayOrder)
            .ToListAsync();
        return Ok(projects);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var project = await _context.Projects.FindAsync(id);
        if (project == null)
            return NotFound(new { message = "Projeto não encontrado." });
        return Ok(project);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] Project project)
    {
        project.CreatedAt = DateTime.UtcNow;
        project.UpdatedAt = DateTime.UtcNow;
        _context.Projects.Add(project);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetById), new { id = project.Id }, project);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] Project updated)
    {
        var project = await _context.Projects.FindAsync(id);
        if (project == null)
            return NotFound(new { message = "Projeto não encontrado." });

        project.Title = updated.Title;
        project.Description = updated.Description;
        project.ThumbnailUrl = updated.ThumbnailUrl;
        project.ProjectUrl = updated.ProjectUrl;
        project.RepositoryUrl = updated.RepositoryUrl;
        project.Technologies = updated.Technologies;
        project.Featured = updated.Featured;
        project.DisplayOrder = updated.DisplayOrder;
        project.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();
        return Ok(project);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var project = await _context.Projects.FindAsync(id);
        if (project == null)
            return NotFound(new { message = "Projeto não encontrado." });

        _context.Projects.Remove(project);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}
