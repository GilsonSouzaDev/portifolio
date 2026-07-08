using Microsoft.AspNetCore.Mvc;
using Moq;
using Portfolio.API.Controllers;
using Portfolio.API.DTOs;
using Portfolio.API.Models;
using Portfolio.API.Services.Interfaces;
using Xunit;

namespace Portfolio.API.Tests;

public class ProjectsControllerTests
{
    [Fact]
    public async Task GetAll_ShouldReturnOkWithProjects()
    {
        // Arrange
        var mockService = new Mock<IProjectsService>();
        mockService.Setup(s => s.GetAllProjectsAsync())
            .ReturnsAsync(new List<ProjectDto> { new ProjectDto { Id = 1, Title = "Test" } });

        var controller = new ProjectsController(mockService.Object);

        // Act
        var result = await controller.GetAll();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var projects = Assert.IsAssignableFrom<IEnumerable<ProjectDto>>(okResult.Value);
        Assert.Single(projects);
    }

    [Fact]
    public async Task Create_ShouldReturnCreatedAtAction()
    {
        // Arrange
        var mockService = new Mock<IProjectsService>();
        var request = new CreateOrUpdateProjectRequest { Title = "New" };
        var createdProject = new ProjectDto { Id = 1, Title = "New" };
        
        mockService.Setup(s => s.CreateProjectAsync(request))
            .ReturnsAsync(createdProject);

        var controller = new ProjectsController(mockService.Object);

        // Act
        var result = await controller.Create(request);

        // Assert
        var createdResult = Assert.IsType<CreatedAtActionResult>(result);
        Assert.Equal(nameof(ProjectsController.GetAll), createdResult.ActionName);
        var project = Assert.IsType<ProjectDto>(createdResult.Value);
        Assert.Equal(1, project.Id);
    }

    [Fact]
    public async Task Update_ShouldReturnOk_WhenProjectExists()
    {
        // Arrange
        var mockService = new Mock<IProjectsService>();
        var request = new CreateOrUpdateProjectRequest { Title = "Updated" };
        var updatedProject = new ProjectDto { Id = 1, Title = "Updated" };

        mockService.Setup(s => s.UpdateProjectAsync(1, request))
            .ReturnsAsync(updatedProject);

        var controller = new ProjectsController(mockService.Object);

        // Act
        var result = await controller.Update(1, request);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var project = Assert.IsType<ProjectDto>(okResult.Value);
        Assert.Equal("Updated", project.Title);
    }

    [Fact]
    public async Task Update_ShouldReturnNotFound_WhenProjectDoesNotExist()
    {
        // Arrange
        var mockService = new Mock<IProjectsService>();
        var request = new CreateOrUpdateProjectRequest { Title = "Updated" };

        mockService.Setup(s => s.UpdateProjectAsync(999, request))
            .ReturnsAsync((ProjectDto?)null);

        var controller = new ProjectsController(mockService.Object);

        // Act
        var result = await controller.Update(999, request);

        // Assert
        Assert.IsType<NotFoundObjectResult>(result);
    }

    [Fact]
    public async Task Delete_ShouldReturnNoContent_WhenProjectExists()
    {
        // Arrange
        var mockService = new Mock<IProjectsService>();
        mockService.Setup(s => s.DeleteProjectAsync(1)).ReturnsAsync(true);

        var controller = new ProjectsController(mockService.Object);

        // Act
        var result = await controller.Delete(1);

        // Assert
        Assert.IsType<NoContentResult>(result);
    }

    [Fact]
    public async Task Delete_ShouldReturnNotFound_WhenProjectDoesNotExist()
    {
        // Arrange
        var mockService = new Mock<IProjectsService>();
        mockService.Setup(s => s.DeleteProjectAsync(999)).ReturnsAsync(false);

        var controller = new ProjectsController(mockService.Object);

        // Act
        var result = await controller.Delete(999);

        // Assert
        Assert.IsType<NotFoundObjectResult>(result);
    }
}
