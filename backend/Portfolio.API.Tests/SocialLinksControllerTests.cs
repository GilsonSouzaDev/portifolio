using Microsoft.AspNetCore.Mvc;
using Moq;
using Portfolio.API.Controllers;
using Portfolio.API.DTOs;
using Portfolio.API.Services.Interfaces;
using Xunit;

namespace Portfolio.API.Tests;

public class SocialLinksControllerTests
{
    [Fact]
    public async Task GetAll_ShouldReturnOkWithLinks()
    {
        // Arrange
        var mockService = new Mock<ISocialLinksService>();
        mockService.Setup(s => s.GetAllSocialLinksAsync())
            .ReturnsAsync(new List<SocialLinkDto> { new SocialLinkDto { Id = 1, Platform = "GitHub" } });

        var controller = new SocialLinksController(mockService.Object);

        // Act
        var result = await controller.GetAll();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var links = Assert.IsAssignableFrom<IEnumerable<SocialLinkDto>>(okResult.Value);
        Assert.Single(links);
    }

    [Fact]
    public async Task Create_ShouldReturnCreatedAtAction()
    {
        // Arrange
        var mockService = new Mock<ISocialLinksService>();
        var request = new CreateOrUpdateSocialLinkRequest { Platform = "LinkedIn" };
        var createdLink = new SocialLinkDto { Id = 1, Platform = "LinkedIn" };
        
        mockService.Setup(s => s.CreateSocialLinkAsync(request))
            .ReturnsAsync(createdLink);

        var controller = new SocialLinksController(mockService.Object);

        // Act
        var result = await controller.Create(request);

        // Assert
        var createdResult = Assert.IsType<CreatedAtActionResult>(result);
        Assert.Equal(nameof(SocialLinksController.GetAll), createdResult.ActionName);
        var link = Assert.IsType<SocialLinkDto>(createdResult.Value);
        Assert.Equal(1, link.Id);
    }

    [Fact]
    public async Task Update_ShouldReturnOk_WhenLinkExists()
    {
        // Arrange
        var mockService = new Mock<ISocialLinksService>();
        var request = new CreateOrUpdateSocialLinkRequest { Platform = "Updated" };
        var updatedLink = new SocialLinkDto { Id = 1, Platform = "Updated" };

        mockService.Setup(s => s.UpdateSocialLinkAsync(1, request))
            .ReturnsAsync(updatedLink);

        var controller = new SocialLinksController(mockService.Object);

        // Act
        var result = await controller.Update(1, request);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var link = Assert.IsType<SocialLinkDto>(okResult.Value);
        Assert.Equal("Updated", link.Platform);
    }

    [Fact]
    public async Task Update_ShouldReturnNotFound_WhenLinkDoesNotExist()
    {
        // Arrange
        var mockService = new Mock<ISocialLinksService>();
        var request = new CreateOrUpdateSocialLinkRequest { Platform = "Updated" };

        mockService.Setup(s => s.UpdateSocialLinkAsync(999, request))
            .ReturnsAsync((SocialLinkDto?)null);

        var controller = new SocialLinksController(mockService.Object);

        // Act
        var result = await controller.Update(999, request);

        // Assert
        Assert.IsType<NotFoundObjectResult>(result);
    }

    [Fact]
    public async Task Delete_ShouldReturnNoContent_WhenLinkExists()
    {
        // Arrange
        var mockService = new Mock<ISocialLinksService>();
        mockService.Setup(s => s.DeleteSocialLinkAsync(1)).ReturnsAsync(true);

        var controller = new SocialLinksController(mockService.Object);

        // Act
        var result = await controller.Delete(1);

        // Assert
        Assert.IsType<NoContentResult>(result);
    }

    [Fact]
    public async Task Delete_ShouldReturnNotFound_WhenLinkDoesNotExist()
    {
        // Arrange
        var mockService = new Mock<ISocialLinksService>();
        mockService.Setup(s => s.DeleteSocialLinkAsync(999)).ReturnsAsync(false);

        var controller = new SocialLinksController(mockService.Object);

        // Act
        var result = await controller.Delete(999);

        // Assert
        Assert.IsType<NotFoundObjectResult>(result);
    }
}
