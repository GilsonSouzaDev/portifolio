using Portfolio.API.DTOs;
using Portfolio.API.Models;
using Portfolio.API.Repositories.Interfaces;
using Portfolio.API.Services.Interfaces;

namespace Portfolio.API.Services.Implementations;

public class ProfileService : IProfileService
{
    private readonly IRepository<Profile> _repository;

    public ProfileService(IRepository<Profile> repository)
    {
        _repository = repository;
    }

    public async Task<ProfileDto?> GetProfileAsync()
    {
        var profile = await _repository.FirstOrDefaultAsync();
        if (profile == null) return null;

        return new ProfileDto
        {
            Id = profile.Id,
            Name = profile.Name,
            Title = profile.Title,
            Bio = profile.Bio,
            AvatarUrl = profile.AvatarUrl,
            ResumeUrl = profile.ResumeUrl
        };
    }

    public async Task<ProfileDto?> UpdateProfileAsync(UpdateProfileRequest request)
    {
        var profile = await _repository.FirstOrDefaultAsync();
        if (profile == null) return null;

        profile.Name = request.Name;
        profile.Title = request.Title;
        profile.Bio = request.Bio;
        profile.AvatarUrl = request.AvatarUrl;
        profile.ResumeUrl = request.ResumeUrl;
        profile.UpdatedAt = DateTime.UtcNow;

        await _repository.UpdateAsync(profile);

        return new ProfileDto
        {
            Id = profile.Id,
            Name = profile.Name,
            Title = profile.Title,
            Bio = profile.Bio,
            AvatarUrl = profile.AvatarUrl,
            ResumeUrl = profile.ResumeUrl
        };
    }
}
