using Microsoft.EntityFrameworkCore;
using Portfolio.API.Data;
using Portfolio.API.DTOs;
using Portfolio.API.Models;
using Portfolio.API.Services.Email;
using Portfolio.API.Services.Interfaces;

namespace Portfolio.API.Services.Implementations;

public class ContactService : IContactService
{
    private readonly AppDbContext _context;
    private readonly IEmailService _emailService;

    public ContactService(AppDbContext context, IEmailService emailService)
    {
        _context = context;
        _emailService = emailService;
    }

    public async Task<bool> SendMessageAsync(SendContactMessageRequest request)
    {
        var message = new ContactMessage
        {
            Name = request.Name,
            Email = request.Email,
            Subject = request.Subject,
            Message = request.Message,
            CreatedAt = DateTime.UtcNow,
            IsRead = false
        };

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

        return true;
    }

    public async Task<IEnumerable<ContactMessageDto>> GetMessagesAsync()
    {
        return await _context.ContactMessages
            .OrderByDescending(m => m.CreatedAt)
            .Select(m => new ContactMessageDto
            {
                Id = m.Id,
                Name = m.Name,
                Email = m.Email,
                Subject = m.Subject,
                Message = m.Message,
                IsRead = m.IsRead,
                CreatedAt = m.CreatedAt
            })
            .ToListAsync();
    }
}
