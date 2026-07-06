using Microsoft.EntityFrameworkCore;
using Moq;
using Portfolio.API.Data;
using Portfolio.API.DTOs;
using Portfolio.API.Services.Email;
using Portfolio.API.Services.Implementations;
using Xunit;

namespace Portfolio.API.Tests;

public class ContactServiceTests
{
    private AppDbContext GetInMemoryDbContext()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        return new AppDbContext(options);
    }

    [Fact]
    public async Task SendMessageAsync_ShouldReturnSuccess_WhenEmailSent()
    {
        // Arrange
        var context = GetInMemoryDbContext();
        var mockEmailService = new Mock<IEmailService>();

        mockEmailService
            .Setup(x => x.SendEmailAsync(It.IsAny<string>(), It.IsAny<string>()))
            .ReturnsAsync(true);

        var service = new ContactService(context, mockEmailService.Object);
        var request = new SendContactMessageRequest
        {
            Name = "John Doe",
            Email = "john@example.com",
            Subject = "Test Subject",
            Message = "Test Message"
        };

        // Act
        var result = await service.SendMessageAsync(request);

        // Assert
        Assert.True(result);
        mockEmailService.Verify(
            x => x.SendEmailAsync(
                It.Is<string>(s => s.Contains("Test Subject") && s.Contains("John Doe")), 
                It.Is<string>(s => s.Contains("Test Message"))
            ), 
            Times.Once
        );
        
        Assert.Equal(1, await context.ContactMessages.CountAsync());
    }
}
