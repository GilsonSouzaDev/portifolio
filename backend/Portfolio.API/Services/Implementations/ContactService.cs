using Microsoft.EntityFrameworkCore;
using Portfolio.API.DTOs;
using Portfolio.API.Models;
using Portfolio.API.Services.Email;
using Portfolio.API.Services.Interfaces;
using Portfolio.API.Repositories.Interfaces;

namespace Portfolio.API.Services.Implementations;

public class ContactService : IContactService
{
    private readonly IRepository<ContactMessage> _repository;
    private readonly IEmailService _emailService;

    public ContactService(IRepository<ContactMessage> repository, IEmailService emailService)
    {
        _repository = repository;
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

        await _repository.AddAsync(message);

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
        var messages = await _repository.GetAllAsync(null, q => q.OrderByDescending(m => m.CreatedAt));
        return messages.Select(m => new ContactMessageDto
        {
            Id = m.Id,
            Name = m.Name,
            Email = m.Email,
            Subject = m.Subject,
            Message = m.Message,
            IsRead = m.IsRead,
            CreatedAt = m.CreatedAt
        });
    }
}
