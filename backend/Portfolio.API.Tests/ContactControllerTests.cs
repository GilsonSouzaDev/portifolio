using Microsoft.AspNetCore.Mvc;
using Moq;
using Portfolio.API.Controllers;
using Portfolio.API.DTOs;
using Portfolio.API.Services.Interfaces;
using Xunit;

namespace Portfolio.API.Tests;

public class ContactControllerTests
{
    [Fact]
    public async Task GetMessages_ShouldReturnOkWithMessages()
    {
        // Arrange
        var mockService = new Mock<IContactService>();
        mockService.Setup(s => s.GetMessagesAsync())
            .ReturnsAsync(new List<ContactMessageDto> { new ContactMessageDto { Id = 1, Name = "Test" } });

        var controller = new ContactController(mockService.Object);

        // Act
        var result = await controller.GetMessages();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var messages = Assert.IsAssignableFrom<IEnumerable<ContactMessageDto>>(okResult.Value);
        Assert.Single(messages);
    }

    [Fact]
    public async Task SendMessage_ShouldReturnOk_WhenSuccess()
    {
        // Arrange
        var mockService = new Mock<IContactService>();
        var request = new SendContactMessageRequest { Name = "Test", Email = "test@test.com", Message = "Msg" };
        mockService.Setup(s => s.SendMessageAsync(request))
            .ReturnsAsync(true);

        var controller = new ContactController(mockService.Object);

        // Act
        var result = await controller.Send(request);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
    }

    [Fact]
    public async Task SendMessage_ShouldReturnBadRequest_WhenRequestIsInvalid()
    {
        // Arrange
        var mockService = new Mock<IContactService>();
        var request = new SendContactMessageRequest { Name = "Test" }; // Missing email and message

        var controller = new ContactController(mockService.Object);

        // Act
        var result = await controller.Send(request);

        // Assert
        Assert.IsType<BadRequestObjectResult>(result);
    }
}
