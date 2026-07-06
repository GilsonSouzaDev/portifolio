using Microsoft.AspNetCore.Mvc;

namespace Portfolio.API.Controllers;

[ApiController]
[Route("api/images")]
public class ImagesController : ControllerBase
{
    private readonly IWebHostEnvironment _env;
    private readonly IConfiguration _configuration;
    private readonly IHttpClientFactory _httpClientFactory;

    public ImagesController(IWebHostEnvironment env, IConfiguration configuration, IHttpClientFactory httpClientFactory)
    {
        _env = env;
        _configuration = configuration;
        _httpClientFactory = httpClientFactory;
    }

    [HttpPost("upload")]
    public async Task<IActionResult> Upload(IFormFile file)
    {
        if (file == null || file.Length == 0)
            return BadRequest(new { message = "Nenhum arquivo enviado." });

        var allowedTypes = new[] { "image/jpeg", "image/png", "image/webp" };
        if (!allowedTypes.Contains(file.ContentType))
            return BadRequest(new { message = "Tipo de arquivo não permitido. Use JPEG, PNG ou WebP." });

        var uploadsFolder = Path.Combine(_env.WebRootPath ?? "wwwroot", "images");
        Directory.CreateDirectory(uploadsFolder);

        var fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
        var filePath = Path.Combine(uploadsFolder, fileName);

        using (var stream = new FileStream(filePath, FileMode.Create))
        {
            await file.CopyToAsync(stream);
        }

        var url = $"{Request.Scheme}://{Request.Host}/images/{fileName}";
        return Ok(new { url, message = "Upload realizado com sucesso." });
    }

    [HttpPost("remove-bg")]
    public async Task<IActionResult> RemoveBackground(IFormFile file)
    {
        if (file == null || file.Length == 0)
            return BadRequest(new { message = "Nenhum arquivo enviado." });

        var apiKey = _configuration["RemoveBg:ApiKey"];
        if (string.IsNullOrEmpty(apiKey))
            return StatusCode(500, new { message = "API Key do remove.bg não configurada." });

        using var httpClient = _httpClientFactory.CreateClient("RemoveBgClient");
        httpClient.DefaultRequestHeaders.Add("X-Api-Key", apiKey);

        using var content = new MultipartFormDataContent();
        using var fileStream = file.OpenReadStream();
        content.Add(new StreamContent(fileStream), "image_file", file.FileName);

        var response = await httpClient.PostAsync("https://api.remove.bg/v1.0/removebg", content);

        if (!response.IsSuccessStatusCode)
            return StatusCode(500, new { message = "Falha ao remover fundo da imagem." });

        var imageBytes = await response.Content.ReadAsByteArrayAsync();

        var uploadsFolder = Path.Combine(_env.WebRootPath ?? "wwwroot", "images");
        Directory.CreateDirectory(uploadsFolder);

        var fileName = $"{Guid.NewGuid()}.png";
        var filePath = Path.Combine(uploadsFolder, fileName);
        await System.IO.File.WriteAllBytesAsync(filePath, imageBytes);

        var url = $"{Request.Scheme}://{Request.Host}/images/{fileName}";
        return Ok(new { url, message = "Fundo removido com sucesso." });
    }
}