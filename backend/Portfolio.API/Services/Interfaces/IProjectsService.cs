using Portfolio.API.DTOs;

namespace Portfolio.API.Services.Interfaces;

public interface IProjectsService
{
    Task<IEnumerable<ProjectDto>> GetAllProjectsAsync();
    Task<ProjectDto> CreateProjectAsync(CreateOrUpdateProjectRequest request);
    Task<ProjectDto?> UpdateProjectAsync(int id, CreateOrUpdateProjectRequest request);
    Task<bool> DeleteProjectAsync(int id);
}
