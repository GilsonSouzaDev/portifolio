using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Moq;
using Portfolio.API.Controllers;
using System.Text;
using Xunit;
using Moq.Protected;
using System.Net;

namespace Portfolio.API.Tests;

public class ImagesControllerTests
{
    private readonly Mock<IWebHostEnvironment> _mockEnv;
    private readonly Mock<IConfiguration> _mockConfig;
    private readonly Mock<IHttpClientFactory> _mockHttpFactory;

    public ImagesControllerTests()
    {
        _mockEnv = new Mock<IWebHostEnvironment>();
        _mockEnv.Setup(e => e.WebRootPath).Returns(Path.GetTempPath());

        var inMemorySettings = new Dictionary<string, string> {
            {"RemoveBg:ApiKey", "test-key"}
        };

        _mockConfig = new Mock<IConfiguration>();
        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(inMemorySettings)
            .Build();

        _mockConfig.Setup(c => c["RemoveBg:ApiKey"]).Returns(configuration["RemoveBg:ApiKey"]);

        _mockHttpFactory = new Mock<IHttpClientFactory>();
    }

    [Fact]
    public async Task Upload_ShouldReturnBadRequest_WhenFileIsNull()
    {
        var controller = new ImagesController(_mockEnv.Object, _mockConfig.Object, _mockHttpFactory.Object);
        var result = await controller.Upload(null!);
        Assert.IsType<BadRequestObjectResult>(result);
    }

    [Fact]
    public async Task Upload_ShouldReturnBadRequest_WhenFileTypeIsInvalid()
    {
        var mockFile = new Mock<IFormFile>();
        mockFile.Setup(f => f.Length).Returns(100);
        mockFile.Setup(f => f.ContentType).Returns("text/plain");

        var controller = new ImagesController(_mockEnv.Object, _mockConfig.Object, _mockHttpFactory.Object);
        var result = await controller.Upload(mockFile.Object);
        Assert.IsType<BadRequestObjectResult>(result);
    }

    [Fact]
    public async Task Upload_ShouldReturnOk_WhenFileIsValid()
    {
        var mockFile = new Mock<IFormFile>();
        var content = "fake image content";
        var fileName = "test.jpg";
        var ms = new MemoryStream(Encoding.UTF8.GetBytes(content));
        
        mockFile.Setup(f => f.OpenReadStream()).Returns(ms);
        mockFile.Setup(f => f.FileName).Returns(fileName);
        mockFile.Setup(f => f.Length).Returns(ms.Length);
        mockFile.Setup(f => f.ContentType).Returns("image/jpeg");
        mockFile.Setup(f => f.CopyToAsync(It.IsAny<Stream>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        var controller = new ImagesController(_mockEnv.Object, _mockConfig.Object, _mockHttpFactory.Object);

        // Mock Request context for Request.Scheme and Request.Host
        var httpContext = new DefaultHttpContext();
        httpContext.Request.Scheme = "http";
        httpContext.Request.Host = new HostString("localhost");
        controller.ControllerContext = new ControllerContext()
        {
            HttpContext = httpContext
        };

        var result = await controller.Upload(mockFile.Object);

        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.NotNull(okResult.Value);
    }

    [Fact]
    public async Task RemoveBackground_ShouldReturnOk_WhenApiCallSucceeds()
    {
        // Setup HttpClient mock
        var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
        mockHttpMessageHandler.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new ByteArrayContent(new byte[] { 1, 2, 3 })
            });

        var client = new HttpClient(mockHttpMessageHandler.Object);
        _mockHttpFactory.Setup(f => f.CreateClient(It.IsAny<string>())).Returns(client);

        var mockFile = new Mock<IFormFile>();
        mockFile.Setup(f => f.Length).Returns(100);
        mockFile.Setup(f => f.FileName).Returns("test.jpg");
        var ms = new MemoryStream(Encoding.UTF8.GetBytes("fake"));
        mockFile.Setup(f => f.OpenReadStream()).Returns(ms);

        var controller = new ImagesController(_mockEnv.Object, _mockConfig.Object, _mockHttpFactory.Object);
        var httpContext = new DefaultHttpContext();
        httpContext.Request.Scheme = "http";
        httpContext.Request.Host = new HostString("localhost");
        controller.ControllerContext = new ControllerContext { HttpContext = httpContext };

        var result = await controller.RemoveBackground(mockFile.Object);

        var okResult = Assert.IsType<OkObjectResult>(result);
    }
}
