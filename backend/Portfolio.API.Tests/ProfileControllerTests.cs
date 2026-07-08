using Microsoft.AspNetCore.Mvc;
using Moq;
using Portfolio.API.Controllers;
using Portfolio.API.DTOs;
using Portfolio.API.Services.Interfaces;
using Xunit;

namespace Portfolio.API.Tests;

public class ProfileControllerTests
{
    [Fact]
    public async Task Get_ShouldReturnOkWithProfile_WhenProfileExists()
    {
        // Arrange
        var mockService = new Mock<IProfileService>();
        mockService.Setup(s => s.GetProfileAsync())
            .ReturnsAsync(new ProfileDto { Name = "Test" });

        var controller = new ProfileController(mockService.Object);

        // Act
        var result = await controller.Get();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var profile = Assert.IsType<ProfileDto>(okResult.Value);
        Assert.Equal("Test", profile.Name);
    }

    [Fact]
    public async Task Get_ShouldReturnNotFound_WhenProfileDoesNotExist()
    {
        // Arrange
        var mockService = new Mock<IProfileService>();
        mockService.Setup(s => s.GetProfileAsync())
            .ReturnsAsync((ProfileDto?)null);

        var controller = new ProfileController(mockService.Object);

        // Act
        var result = await controller.Get();

        // Assert
        Assert.IsType<NotFoundObjectResult>(result);
    }

    [Fact]
    public async Task Update_ShouldReturnOk_WhenUpdateSucceeds()
    {
        // Arrange
        var mockService = new Mock<IProfileService>();
        var request = new UpdateProfileRequest { Name = "Updated" };
        var updatedProfile = new ProfileDto { Name = "Updated" };
        
        mockService.Setup(s => s.UpdateProfileAsync(request))
            .ReturnsAsync(updatedProfile);

        var controller = new ProfileController(mockService.Object);

        // Act
        var result = await controller.Update(request);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var profile = Assert.IsType<ProfileDto>(okResult.Value);
        Assert.Equal("Updated", profile.Name);
    }
}
