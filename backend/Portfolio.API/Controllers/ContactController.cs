using Microsoft.AspNetCore.Mvc;
using Portfolio.API.DTOs;
using Portfolio.API.Services.Interfaces;

namespace Portfolio.API.Controllers;

[ApiController]
[Route("api/contact")]
public class ContactController : ControllerBase
{
    private readonly IContactService _contactService;

    public ContactController(IContactService contactService)
    {
        _contactService = contactService;
    }

    [HttpPost]
    public async Task<IActionResult> Send([FromBody] SendContactMessageRequest messageRequest)
    {
        if (string.IsNullOrWhiteSpace(messageRequest.Name) ||
            string.IsNullOrWhiteSpace(messageRequest.Email) ||
            string.IsNullOrWhiteSpace(messageRequest.Message))
            return BadRequest(new { message = "Nome, e-mail e mensagem são obrigatórios." });

        await _contactService.SendMessageAsync(messageRequest);

        return Ok(new { message = "Mensagem enviada com sucesso." });
    }

    [HttpGet("messages")]
    public async Task<IActionResult> GetMessages()
    {
        var messages = await _contactService.GetMessagesAsync();
        return Ok(messages);
    }
}