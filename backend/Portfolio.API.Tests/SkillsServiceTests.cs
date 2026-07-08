using Microsoft.EntityFrameworkCore;
using Portfolio.API.Data;
using Portfolio.API.DTOs;
using Portfolio.API.Models;
using Portfolio.API.Services.Implementations;
using Xunit;

namespace Portfolio.API.Tests;

public class SkillsServiceTests
{
    private AppDbContext GetInMemoryDbContext()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        return new AppDbContext(options);
    }

    [Fact]
    public async Task GetAllSkillsAsync_ShouldReturnSortedSkills()
    {
        using var context = GetInMemoryDbContext();
        context.Skills.AddRange(
            new Skill { Id = 1, Name = "C#", Category = SkillCategory.Hard, DisplayOrder = 2 },
            new Skill { Id = 2, Name = "Communication", Category = SkillCategory.Soft, DisplayOrder = 1 }
        );
        await context.SaveChangesAsync();

        var service = new SkillsService(context);

        var result = await service.GetAllSkillsAsync();

        Assert.Equal(2, result.Count());
        Assert.Equal("Communication", result.First().Name);
        Assert.Equal("C#", result.Last().Name);
    }

    [Fact]
    public async Task CreateSkillAsync_ShouldAddSkill()
    {
        using var context = GetInMemoryDbContext();
        var service = new SkillsService(context);
        var request = new CreateOrUpdateSkillRequest { Name = "New Skill", Category = SkillCategory.Badge };

        var result = await service.CreateSkillAsync(request);

        Assert.NotNull(result);
        Assert.Equal("New Skill", result.Name);
        Assert.Equal(1, await context.Skills.CountAsync());
    }

    [Fact]
    public async Task UpdateSkillAsync_ShouldModifySkill()
    {
        using var context = GetInMemoryDbContext();
        context.Skills.Add(new Skill { Id = 1, Name = "Old", Category = SkillCategory.Hard });
        await context.SaveChangesAsync();

        var service = new SkillsService(context);
        var request = new CreateOrUpdateSkillRequest { Name = "Updated", Category = SkillCategory.Hard };

        var result = await service.UpdateSkillAsync(1, request);

        Assert.NotNull(result);
        Assert.Equal("Updated", result.Name);
    }

    [Fact]
    public async Task UpdateSkillAsync_ShouldReturnNull_WhenNotExists()
    {
        using var context = GetInMemoryDbContext();
        var service = new SkillsService(context);
        var request = new CreateOrUpdateSkillRequest { Name = "Updated", Category = SkillCategory.Hard };
        var result = await service.UpdateSkillAsync(999, request);
        Assert.Null(result);
    }

    [Fact]
    public async Task DeleteSkillAsync_ShouldRemoveSkill()
    {
        using var context = GetInMemoryDbContext();
        context.Skills.Add(new Skill { Id = 1, Name = "To Delete", Category = SkillCategory.Badge });
        await context.SaveChangesAsync();

        var service = new SkillsService(context);
        var result = await service.DeleteSkillAsync(1);

        Assert.True(result);
        Assert.Equal(0, await context.Skills.CountAsync());
    }

    [Fact]
    public async Task DeleteSkillAsync_ShouldReturnFalse_WhenNotExists()
    {
        using var context = GetInMemoryDbContext();
        var service = new SkillsService(context);
        var result = await service.DeleteSkillAsync(999);
        Assert.False(result);
    }
}
