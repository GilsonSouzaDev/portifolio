using Microsoft.EntityFrameworkCore;
using Portfolio.API.Data;
using Portfolio.API.DTOs;
using Portfolio.API.Models;
using Portfolio.API.Services.Interfaces;

namespace Portfolio.API.Services.Implementations;

public class SocialLinksService : ISocialLinksService
{
    private readonly AppDbContext _context;

    public SocialLinksService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<SocialLinkDto>> GetAllSocialLinksAsync()
    {
        return await _context.SocialLinks
            .OrderBy(sl => sl.DisplayOrder)
            .Select(sl => new SocialLinkDto
            {
                Id = sl.Id,
                Platform = sl.Platform,
                Url = sl.Url,
                IconUrl = sl.IconUrl,
                DisplayOrder = sl.DisplayOrder
            })
            .ToListAsync();
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

        _context.SocialLinks.Add(socialLink);
        await _context.SaveChangesAsync();

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
        var socialLink = await _context.SocialLinks.FindAsync(id);
        if (socialLink == null) return null;

        socialLink.Platform = request.Platform;
        socialLink.Url = request.Url;
        socialLink.IconUrl = request.IconUrl;
        socialLink.DisplayOrder = request.DisplayOrder;
        socialLink.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

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
        var socialLink = await _context.SocialLinks.FindAsync(id);
        if (socialLink == null) return false;

        _context.SocialLinks.Remove(socialLink);
        await _context.SaveChangesAsync();
        return true;
    }
}
