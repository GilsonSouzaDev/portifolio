using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Portfolio.API.Data;
using Portfolio.API.Models;
using Portfolio.API.Services.Email;

namespace Portfolio.API.Controllers;

[ApiController]
[Route("api/contact")]
public class ContactController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly IEmailService _emailService;

    public ContactController(AppDbContext context, IEmailService emailService)
    {
        _context = context;
        _emailService = emailService;
    }

    [HttpPost]
    public async Task<IActionResult> Send([FromBody] ContactMessage message)
    {
        if (string.IsNullOrWhiteSpace(message.Name) ||
            string.IsNullOrWhiteSpace(message.Email) ||
            string.IsNullOrWhiteSpace(message.Message))
            return BadRequest(new { message = "Nome, e-mail e mensagem são obrigatórios." });

        message.CreatedAt = DateTime.UtcNow;
        message.IsRead = false;

        _context.ContactMessages.Add(message);
        await _context.SaveChangesAsync();

        // Construir o corpo do e-mail
        var body = $"Você recebeu uma nova mensagem de contato do seu Portfólio!\n\n" +
                   $"Nome: {message.Name}\n" +
                   $"Email: {message.Email}\n" +
                   $"Assunto: {message.Subject}\n\n" +
                   $"Mensagem:\n{message.Message}";

        // Enviar o e-mail
        await _emailService.SendEmailAsync($"Novo Contato: {message.Subject} - {message.Name}", body);

        return Ok(new { message = "Mensagem enviada com sucesso." });
    }

    [HttpGet("messages")]
    public async Task<IActionResult> GetMessages()
    {
        var messages = await _context.ContactMessages
            .OrderByDescending(m => m.CreatedAt)
            .ToListAsync();
        return Ok(messages);
    }
}