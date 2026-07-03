using Portfolio.API.DTOs;

namespace Portfolio.API.Services.Interfaces;

public interface IContactService
{
    Task<bool> SendMessageAsync(SendContactMessageRequest request);
    Task<IEnumerable<ContactMessageDto>> GetMessagesAsync();
}
