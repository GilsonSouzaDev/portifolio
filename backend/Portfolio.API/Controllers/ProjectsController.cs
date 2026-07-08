using Microsoft.AspNetCore.Mvc;
using Portfolio.API.DTOs;
using Portfolio.API.Services.Interfaces;

namespace Portfolio.API.Controllers;

[ApiController]
[Route("api/projects")]
public class ProjectsController : ControllerBase
{
    private readonly IProjectsService _projectsService;

    public ProjectsController(IProjectsService projectsService)
    {
        _projectsService = projectsService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var projects = await _projectsService.GetAllProjectsAsync();
        return Ok(projects);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateOrUpdateProjectRequest projectRequest)
    {
        var project = await _projectsService.CreateProjectAsync(projectRequest);
        return CreatedAtAction(nameof(GetAll), new { id = project.Id }, project);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] CreateOrUpdateProjectRequest projectRequest)
    {
        var project = await _projectsService.UpdateProjectAsync(id, projectRequest);

        if (project == null)
            return NotFound(new { message = "Projeto não encontrado." });

        return Ok(project);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _projectsService.DeleteProjectAsync(id);

        if (!result)
            return NotFound(new { message = "Projeto não encontrado." });

        return NoContent();
    }
}
