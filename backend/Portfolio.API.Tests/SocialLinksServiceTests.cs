using Microsoft.EntityFrameworkCore;
using Portfolio.API.Data;
using Portfolio.API.DTOs;
using Portfolio.API.Models;
using Portfolio.API.Services.Implementations;
using Xunit;

namespace Portfolio.API.Tests;

public class SocialLinksServiceTests
{
    private AppDbContext GetInMemoryDbContext()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        return new AppDbContext(options);
    }

    [Fact]
    public async Task GetAllLinksAsync_ShouldReturnSortedLinks()
    {
        using var context = GetInMemoryDbContext();
        context.SocialLinks.AddRange(
            new SocialLink { Id = 1, Platform = "GitHub", Url = "github.com", DisplayOrder = 2 },
            new SocialLink { Id = 2, Platform = "LinkedIn", Url = "linkedin.com", DisplayOrder = 1 }
        );
        await context.SaveChangesAsync();

        var service = new SocialLinksService(context);
        var result = await service.GetAllSocialLinksAsync();

        Assert.Equal(2, result.Count());
        Assert.Equal("LinkedIn", result.First().Platform);
        Assert.Equal("GitHub", result.Last().Platform);
    }

    [Fact]
    public async Task CreateLinkAsync_ShouldAddLink()
    {
        using var context = GetInMemoryDbContext();
        var service = new SocialLinksService(context);
        var request = new CreateOrUpdateSocialLinkRequest { Platform = "Twitter", Url = "twitter.com" };

        var result = await service.CreateSocialLinkAsync(request);

        Assert.NotNull(result);
        Assert.Equal("Twitter", result.Platform);
        Assert.Equal(1, await context.SocialLinks.CountAsync());
    }

    [Fact]
    public async Task UpdateLinkAsync_ShouldModifyLink()
    {
        using var context = GetInMemoryDbContext();
        context.SocialLinks.Add(new SocialLink { Id = 1, Platform = "Old", Url = "old.com" });
        await context.SaveChangesAsync();

        var service = new SocialLinksService(context);
        var request = new CreateOrUpdateSocialLinkRequest { Platform = "Updated", Url = "updated.com" };

        var result = await service.UpdateSocialLinkAsync(1, request);

        Assert.NotNull(result);
        Assert.Equal("Updated", result.Platform);
    }

    [Fact]
    public async Task UpdateLinkAsync_ShouldReturnNull_WhenNotExists()
    {
        using var context = GetInMemoryDbContext();
        var service = new SocialLinksService(context);
        var request = new CreateOrUpdateSocialLinkRequest { Platform = "Updated", Url = "updated.com" };
        var result = await service.UpdateSocialLinkAsync(999, request);
        Assert.Null(result);
    }

    [Fact]
    public async Task DeleteLinkAsync_ShouldRemoveLink()
    {
        using var context = GetInMemoryDbContext();
        context.SocialLinks.Add(new SocialLink { Id = 1, Platform = "To Delete", Url = "delete.com" });
        await context.SaveChangesAsync();

        var service = new SocialLinksService(context);
        var result = await service.DeleteSocialLinkAsync(1);

        Assert.True(result);
        Assert.Equal(0, await context.SocialLinks.CountAsync());
    }

    [Fact]
    public async Task DeleteLinkAsync_ShouldReturnFalse_WhenNotExists()
    {
        using var context = GetInMemoryDbContext();
        var service = new SocialLinksService(context);
        var result = await service.DeleteSocialLinkAsync(999);
        Assert.False(result);
    }
}
