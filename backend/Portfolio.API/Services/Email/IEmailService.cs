namespace Portfolio.API.Services.Email;

public interface IEmailService
{
    Task<bool> SendEmailAsync(string subject, string body);
}