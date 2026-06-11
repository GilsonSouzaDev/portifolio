using Microsoft.AspNetCore.Mvc;
using Portfolio.API.Data;
using Portfolio.API.Models;
using Portfolio.API.Services.Email;

namespace Portfolio.API.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly IEmailService _emailService;

    public AuthController(AppDbContext context, IEmailService emailService)
    {
        _context = context;
        _emailService = emailService;
    }

    [HttpPost("request-link")]
    public async Task<IActionResult> RequestLink()
    {
        var code = new Random().Next(100000, 999999).ToString();
        var token = new MagicLinkToken
        {
            Token = code,
            ExpiresAt = DateTime.UtcNow.AddMinutes(15),
            IsUsed = false
        };

        _context.MagicLinkTokens.Add(token);
        await _context.SaveChangesAsync();

        var sent = await _emailService.SendEmailAsync(
            "Seu código de acesso - Portfolio",
            $"Seu código de acesso é: {code}\n\nEste código expira em 15 minutos."
        );

        if (!sent)
            return StatusCode(500, new { message = "Falha ao enviar o código. Tente novamente." });

        return Ok(new { message = "Código enviado para o e-mail cadastrado." });
    }
}
