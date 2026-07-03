using Microsoft.EntityFrameworkCore;
using Portfolio.API.Data;
using Portfolio.API.DTOs;
using Portfolio.API.Services.Interfaces;

namespace Portfolio.API.Services.Implementations;

public class ProfileService : IProfileService
{
    private readonly AppDbContext _context;

    public ProfileService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<ProfileDto?> GetProfileAsync()
    {
        var profile = await _context.Profiles.FirstOrDefaultAsync();
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
        var profile = await _context.Profiles.FirstOrDefaultAsync();
        if (profile == null) return null;

        profile.Name = request.Name;
        profile.Title = request.Title;
        profile.Bio = request.Bio;
        profile.AvatarUrl = request.AvatarUrl;
        profile.ResumeUrl = request.ResumeUrl;
        profile.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

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
