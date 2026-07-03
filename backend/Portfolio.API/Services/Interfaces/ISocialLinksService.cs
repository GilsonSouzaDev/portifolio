using Portfolio.API.DTOs;

namespace Portfolio.API.Services.Interfaces;

public interface ISocialLinksService
{
    Task<IEnumerable<SocialLinkDto>> GetAllSocialLinksAsync();
    Task<SocialLinkDto> CreateSocialLinkAsync(CreateOrUpdateSocialLinkRequest request);
    Task<SocialLinkDto?> UpdateSocialLinkAsync(int id, CreateOrUpdateSocialLinkRequest request);
    Task<bool> DeleteSocialLinkAsync(int id);
}
