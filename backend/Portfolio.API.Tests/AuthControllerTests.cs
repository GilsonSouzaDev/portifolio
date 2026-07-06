using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using Portfolio.API.Controllers;
using Portfolio.API.Data;
using Portfolio.API.Models;
using Portfolio.API.Services.Email;
using Xunit;

namespace Portfolio.API.Tests;

public class AuthControllerTests
{
    private AppDbContext GetInMemoryDbContext()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        return new AppDbContext(options);
    }

    [Fact]
    public async Task RequestLink_ShouldReturnOk_WhenEmailSent()
    {
        // Arrange
        var context = GetInMemoryDbContext();
        var mockEmail = new Mock<IEmailService>();
        mockEmail.Setup(e => e.SendEmailAsync(It.IsAny<string>(), It.IsAny<string>()))
            .ReturnsAsync(true);

        var controller = new AuthController(context, mockEmail.Object);

        // Act
        var result = await controller.RequestLink();

        // Assert
        Assert.IsType<OkObjectResult>(result);
        Assert.Equal(1, await context.MagicLinkTokens.CountAsync());
    }

    [Fact]
    public async Task RequestLink_ShouldReturnInternalServerError_WhenEmailFails()
    {
        // Arrange
        var context = GetInMemoryDbContext();
        var mockEmail = new Mock<IEmailService>();
        mockEmail.Setup(e => e.SendEmailAsync(It.IsAny<string>(), It.IsAny<string>()))
            .ReturnsAsync(false);

        var controller = new AuthController(context, mockEmail.Object);

        // Act
        var result = await controller.RequestLink();

        // Assert
        var statusCodeResult = Assert.IsType<ObjectResult>(result);
        Assert.Equal(500, statusCodeResult.StatusCode);
    }

    [Fact]
    public async Task Verify_ShouldReturnUnauthorized_WhenTokenInvalid()
    {
        // Arrange
        var context = GetInMemoryDbContext();
        var mockEmail = new Mock<IEmailService>();

        var controller = new AuthController(context, mockEmail.Object);

        // Act
        var result = await controller.Verify("invalid");

        // Assert
        Assert.IsType<UnauthorizedObjectResult>(result);
    }

    [Fact]
    public async Task Verify_ShouldReturnOk_WhenTokenValid()
    {
        // Arrange
        var context = GetInMemoryDbContext();
        var mockEmail = new Mock<IEmailService>();
        
        var token = new MagicLinkToken
        {
            Token = "123456",
            ExpiresAt = DateTime.UtcNow.AddMinutes(10),
            IsUsed = false
        };
        context.MagicLinkTokens.Add(token);
        await context.SaveChangesAsync();

        var controller = new AuthController(context, mockEmail.Object);

        // Act
        var result = await controller.Verify("123456");

        // Assert
        Assert.IsType<OkObjectResult>(result);
        var dbToken = await context.MagicLinkTokens.FirstAsync();
        Assert.True(dbToken.IsUsed);
    }

    [Fact]
    public void Logout_ShouldReturnOk()
    {
        // Arrange
        var context = GetInMemoryDbContext();
        var mockEmail = new Mock<IEmailService>();
        var controller = new AuthController(context, mockEmail.Object);

        // Act
        var result = controller.Logout();

        // Assert
        Assert.IsType<OkObjectResult>(result);
    }
}
