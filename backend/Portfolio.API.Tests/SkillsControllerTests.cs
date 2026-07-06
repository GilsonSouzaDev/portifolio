using Microsoft.AspNetCore.Mvc;
using Moq;
using Portfolio.API.Controllers;
using Portfolio.API.DTOs;
using Portfolio.API.Models;
using Portfolio.API.Services.Interfaces;
using Xunit;

namespace Portfolio.API.Tests;

public class SkillsControllerTests
{
    [Fact]
    public async Task GetAll_ShouldReturnOkWithSkills()
    {
        // Arrange
        var mockService = new Mock<ISkillsService>();
        mockService.Setup(s => s.GetAllSkillsAsync())
            .ReturnsAsync(new List<SkillDto> { new SkillDto { Id = 1, Name = "C#" } });

        var controller = new SkillsController(mockService.Object);

        // Act
        var result = await controller.GetAll();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var skills = Assert.IsAssignableFrom<IEnumerable<SkillDto>>(okResult.Value);
        Assert.Single(skills);
    }

    [Fact]
    public async Task Create_ShouldReturnCreatedAtAction()
    {
        // Arrange
        var mockService = new Mock<ISkillsService>();
        var request = new CreateOrUpdateSkillRequest { Name = "New" };
        var createdSkill = new SkillDto { Id = 1, Name = "New" };
        
        mockService.Setup(s => s.CreateSkillAsync(request))
            .ReturnsAsync(createdSkill);

        var controller = new SkillsController(mockService.Object);

        // Act
        var result = await controller.Create(request);

        // Assert
        var createdResult = Assert.IsType<CreatedAtActionResult>(result);
        Assert.Equal(nameof(SkillsController.GetAll), createdResult.ActionName);
        var skill = Assert.IsType<SkillDto>(createdResult.Value);
        Assert.Equal(1, skill.Id);
    }

    [Fact]
    public async Task Update_ShouldReturnOk_WhenSkillExists()
    {
        // Arrange
        var mockService = new Mock<ISkillsService>();
        var request = new CreateOrUpdateSkillRequest { Name = "Updated" };
        var updatedSkill = new SkillDto { Id = 1, Name = "Updated" };

        mockService.Setup(s => s.UpdateSkillAsync(1, request))
            .ReturnsAsync(updatedSkill);

        var controller = new SkillsController(mockService.Object);

        // Act
        var result = await controller.Update(1, request);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var skill = Assert.IsType<SkillDto>(okResult.Value);
        Assert.Equal("Updated", skill.Name);
    }

    [Fact]
    public async Task Update_ShouldReturnNotFound_WhenSkillDoesNotExist()
    {
        // Arrange
        var mockService = new Mock<ISkillsService>();
        var request = new CreateOrUpdateSkillRequest { Name = "Updated" };

        mockService.Setup(s => s.UpdateSkillAsync(999, request))
            .ReturnsAsync((SkillDto?)null);

        var controller = new SkillsController(mockService.Object);

        // Act
        var result = await controller.Update(999, request);

        // Assert
        Assert.IsType<NotFoundObjectResult>(result);
    }

    [Fact]
    public async Task Delete_ShouldReturnNoContent_WhenSkillExists()
    {
        // Arrange
        var mockService = new Mock<ISkillsService>();
        mockService.Setup(s => s.DeleteSkillAsync(1)).ReturnsAsync(true);

        var controller = new SkillsController(mockService.Object);

        // Act
        var result = await controller.Delete(1);

        // Assert
        Assert.IsType<NoContentResult>(result);
    }

    [Fact]
    public async Task Delete_ShouldReturnNotFound_WhenSkillDoesNotExist()
    {
        // Arrange
        var mockService = new Mock<ISkillsService>();
        mockService.Setup(s => s.DeleteSkillAsync(999)).ReturnsAsync(false);

        var controller = new SkillsController(mockService.Object);

        // Act
        var result = await controller.Delete(999);

        // Assert
        Assert.IsType<NotFoundObjectResult>(result);
    }
}
