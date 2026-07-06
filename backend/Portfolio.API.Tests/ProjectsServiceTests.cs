using Microsoft.EntityFrameworkCore;
using Portfolio.API.Data;
using Portfolio.API.DTOs;
using Portfolio.API.Models;
using Portfolio.API.Services.Implementations;
using Xunit;

namespace Portfolio.API.Tests;

public class ProjectsServiceTests
{
    private AppDbContext GetInMemoryDbContext()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        return new AppDbContext(options);
    }

    [Fact]
    public async Task GetAllProjectsAsync_ShouldReturnSortedProjects()
    {
        using var context = GetInMemoryDbContext();
        context.Projects.AddRange(
            new Project { Id = 1, Title = "P1", Description = "D1", DisplayOrder = 2 },
            new Project { Id = 2, Title = "P2", Description = "D2", DisplayOrder = 1 }
        );
        await context.SaveChangesAsync();

        var service = new ProjectsService(context);

        var result = await service.GetAllProjectsAsync();

        Assert.Equal(2, result.Count());
        Assert.Equal("P2", result.First().Title);
        Assert.Equal("P1", result.Last().Title);
    }

    [Fact]
    public async Task CreateProjectAsync_ShouldAddProject()
    {
        using var context = GetInMemoryDbContext();
        var service = new ProjectsService(context);
        var request = new CreateOrUpdateProjectRequest { Title = "New", Description = "Desc" };

        var result = await service.CreateProjectAsync(request);

        Assert.NotNull(result);
        Assert.Equal("New", result.Title);
        Assert.Equal(1, await context.Projects.CountAsync());
    }

    [Fact]
    public async Task UpdateProjectAsync_ShouldModifyProject()
    {
        using var context = GetInMemoryDbContext();
        context.Projects.Add(new Project { Id = 1, Title = "Old", Description = "OldDesc" });
        await context.SaveChangesAsync();

        var service = new ProjectsService(context);
        var request = new CreateOrUpdateProjectRequest { Title = "Updated", Description = "UpdatedDesc" };

        var result = await service.UpdateProjectAsync(1, request);

        Assert.NotNull(result);
        Assert.Equal("Updated", result.Title);
        Assert.Equal("UpdatedDesc", result.Description);
        var inDb = await context.Projects.FindAsync(1);
        Assert.Equal("Updated", inDb.Title);
    }

    [Fact]
    public async Task UpdateProjectAsync_ShouldReturnNull_WhenNotExists()
    {
        using var context = GetInMemoryDbContext();
        var service = new ProjectsService(context);
        var request = new CreateOrUpdateProjectRequest { Title = "Updated", Description = "UpdatedDesc" };

        var result = await service.UpdateProjectAsync(999, request);

        Assert.Null(result);
    }

    [Fact]
    public async Task DeleteProjectAsync_ShouldRemoveProject_AndReturnTrue()
    {
        using var context = GetInMemoryDbContext();
        context.Projects.Add(new Project { Id = 1, Title = "To Delete", Description = "D" });
        await context.SaveChangesAsync();

        var service = new ProjectsService(context);

        var result = await service.DeleteProjectAsync(1);

        Assert.True(result);
        Assert.Equal(0, await context.Projects.CountAsync());
    }

    [Fact]
    public async Task DeleteProjectAsync_ShouldReturnFalse_WhenNotExists()
    {
        using var context = GetInMemoryDbContext();
        var service = new ProjectsService(context);

        var result = await service.DeleteProjectAsync(999);

        Assert.False(result);
    }
}
