using Microsoft.EntityFrameworkCore;
using Portfolio.API.Data;
using Portfolio.API.DTOs;
using Portfolio.API.Models;
using Portfolio.API.Services.Implementations;
using Xunit;

namespace Portfolio.API.Tests;

public class ProfileServiceTests
{
    private AppDbContext GetInMemoryDbContext()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        return new AppDbContext(options);
    }

    [Fact]
    public async Task GetProfileAsync_ShouldReturnNull_WhenProfileDoesNotExist()
    {
        // Arrange
        using var context = GetInMemoryDbContext();
        var service = new ProfileService(context);

        // Act
        var result = await service.GetProfileAsync();

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task GetProfileAsync_ShouldReturnProfile_WhenItExists()
    {
        // Arrange
        using var context = GetInMemoryDbContext();
        var expectedProfile = new Profile
        {
            Id = 1,
            Name = "John Doe",
            Title = "Software Engineer",
            Bio = "Hello world",
            AvatarUrl = "http://example.com/avatar.png",
            ResumeUrl = "http://example.com/resume.pdf"
        };
        context.Profiles.Add(expectedProfile);
        await context.SaveChangesAsync();

        var service = new ProfileService(context);

        // Act
        var result = await service.GetProfileAsync();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(expectedProfile.Name, result.Name);
        Assert.Equal(expectedProfile.Title, result.Title);
        Assert.Equal(expectedProfile.Bio, result.Bio);
    }

    [Fact]
    public async Task UpdateProfileAsync_ShouldUpdate_AndReturnProfileDto()
    {
        // Arrange
        using var context = GetInMemoryDbContext();
        context.Profiles.Add(new Profile
        {
            Id = 1,
            Name = "Old Name",
            Title = "Old Title",
            Bio = "Old Bio"
        });
        await context.SaveChangesAsync();

        var service = new ProfileService(context);
        var request = new UpdateProfileRequest
        {
            Name = "New Name",
            Title = "New Title",
            Bio = "New Bio",
            AvatarUrl = "new.png",
            ResumeUrl = "new.pdf"
        };

        // Act
        var result = await service.UpdateProfileAsync(request);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("New Name", result.Name);
        Assert.Equal("New Title", result.Title);
        
        var profileInDb = await context.Profiles.FirstAsync();
        Assert.Equal("New Name", profileInDb.Name);
    }
}
