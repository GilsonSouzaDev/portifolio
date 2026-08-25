using Portfolio.API.DTOs;
using Portfolio.API.Models;
using Portfolio.API.Repositories.Interfaces;
using Portfolio.API.Services.Interfaces;

namespace Portfolio.API.Services.Implementations;

public class SocialLinksService : ISocialLinksService
{
    private readonly IRepository<SocialLink> _repository;

    public SocialLinksService(IRepository<SocialLink> repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<SocialLinkDto>> GetAllSocialLinksAsync()
    {
        var links = await _repository.GetAllAsync(null, q => q.OrderBy(sl => sl.DisplayOrder));
        return links.Select(sl => new SocialLinkDto
        {
            Id = sl.Id,
            Platform = sl.Platform,
            Url = sl.Url,
            IconUrl = sl.IconUrl,
            DisplayOrder = sl.DisplayOrder
        });
    }

    public async Task<SocialLinkDto> CreateSocialLinkAsync(CreateOrUpdateSocialLinkRequest request)
    {
        var socialLink = new SocialLink
        {
            Platform = request.Platform,
            Url = request.Url,
            IconUrl = request.IconUrl,
            DisplayOrder = request.DisplayOrder,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        await _repository.AddAsync(socialLink);

        return new SocialLinkDto
        {
            Id = socialLink.Id,
            Platform = socialLink.Platform,
            Url = socialLink.Url,
            IconUrl = socialLink.IconUrl,
            DisplayOrder = socialLink.DisplayOrder
        };
    }

    public async Task<SocialLinkDto?> UpdateSocialLinkAsync(int id, CreateOrUpdateSocialLinkRequest request)
    {
        var socialLink = await _repository.GetByIdAsync(id);
        if (socialLink == null) return null;

        socialLink.Platform = request.Platform;
        socialLink.Url = request.Url;
        socialLink.IconUrl = request.IconUrl;
        socialLink.DisplayOrder = request.DisplayOrder;
        socialLink.UpdatedAt = DateTime.UtcNow;

        await _repository.UpdateAsync(socialLink);

        return new SocialLinkDto
        {
            Id = socialLink.Id,
            Platform = socialLink.Platform,
            Url = socialLink.Url,
            IconUrl = socialLink.IconUrl,
            DisplayOrder = socialLink.DisplayOrder
        };
    }

    public async Task<bool> DeleteSocialLinkAsync(int id)
    {
        var socialLink = await _repository.GetByIdAsync(id);
        if (socialLink == null) return false;

        await _repository.DeleteAsync(socialLink);
        return true;
    }
}
