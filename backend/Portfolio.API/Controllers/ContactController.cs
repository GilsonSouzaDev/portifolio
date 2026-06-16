using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Portfolio.API.Data;
using Portfolio.API.Models;

namespace Portfolio.API.Controllers;

[ApiController]
[Route("api/contact")]
public class ContactController : ControllerBase
{
    private readonly AppDbContext _context;

    public ContactController(AppDbContext context)
    {
        _context = context;
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