using Portfolio.API.DTOs;
using Portfolio.API.Models;
using Portfolio.API.Repositories.Interfaces;
using Portfolio.API.Services.Interfaces;

namespace Portfolio.API.Services.Implementations;

public class ProjectsService : IProjectsService
{
    private readonly IRepository<Project> _repository;

    public ProjectsService(IRepository<Project> repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<ProjectDto>> GetAllProjectsAsync()
    {
        var projects = await _repository.GetAllAsync(null, q => q.OrderBy(p => p.DisplayOrder));
        return projects.Select(p => new ProjectDto
        {
            Id = p.Id,
            Title = p.Title,
            Description = p.Description,
            ThumbnailUrl = p.ThumbnailUrl,
            ProjectUrl = p.ProjectUrl,
            RepositoryUrl = p.RepositoryUrl,
            Technologies = p.Technologies,
            Featured = p.Featured,
            DisplayOrder = p.DisplayOrder
        });
    }

    public async Task<ProjectDto> CreateProjectAsync(CreateOrUpdateProjectRequest request)
    {
        var project = new Project
        {
            Title = request.Title,
            Description = request.Description,
            ThumbnailUrl = request.ThumbnailUrl,
            ProjectUrl = request.ProjectUrl,
            RepositoryUrl = request.RepositoryUrl,
            Technologies = request.Technologies,
            Featured = request.Featured,
            DisplayOrder = request.DisplayOrder,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        await _repository.AddAsync(project);

        return new ProjectDto
        {
            Id = project.Id,
            Title = project.Title,
            Description = project.Description,
            ThumbnailUrl = project.ThumbnailUrl,
            ProjectUrl = project.ProjectUrl,
            RepositoryUrl = project.RepositoryUrl,
            Technologies = project.Technologies,
            Featured = project.Featured,
            DisplayOrder = project.DisplayOrder
        };
    }

    public async Task<ProjectDto?> UpdateProjectAsync(int id, CreateOrUpdateProjectRequest request)
    {
        var project = await _repository.GetByIdAsync(id);
        if (project == null) return null;

        project.Title = request.Title;
        project.Description = request.Description;
        project.ThumbnailUrl = request.ThumbnailUrl;
        project.ProjectUrl = request.ProjectUrl;
        project.RepositoryUrl = request.RepositoryUrl;
        project.Technologies = request.Technologies;
        project.Featured = request.Featured;
        project.DisplayOrder = request.DisplayOrder;
        project.UpdatedAt = DateTime.UtcNow;

        await _repository.UpdateAsync(project);

        return new ProjectDto
        {
            Id = project.Id,
            Title = project.Title,
            Description = project.Description,
            ThumbnailUrl = project.ThumbnailUrl,
            ProjectUrl = project.ProjectUrl,
            RepositoryUrl = project.RepositoryUrl,
            Technologies = project.Technologies,
            Featured = project.Featured,
            DisplayOrder = project.DisplayOrder
        };
    }

    public async Task<bool> DeleteProjectAsync(int id)
    {
        var project = await _repository.GetByIdAsync(id);
        if (project == null) return false;

        await _repository.DeleteAsync(project);
        return true;
    }
}
