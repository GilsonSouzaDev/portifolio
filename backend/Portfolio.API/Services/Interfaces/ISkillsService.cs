using Portfolio.API.DTOs;

namespace Portfolio.API.Services.Interfaces;

public interface ISkillsService
{
    Task<IEnumerable<SkillDto>> GetAllSkillsAsync();
    Task<SkillDto> CreateSkillAsync(CreateOrUpdateSkillRequest request);
    Task<SkillDto?> UpdateSkillAsync(int id, CreateOrUpdateSkillRequest request);
    Task<bool> DeleteSkillAsync(int id);
}
