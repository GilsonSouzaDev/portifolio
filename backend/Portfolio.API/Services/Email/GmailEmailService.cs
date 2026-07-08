using System.Net;
using System.Net.Mail;

namespace Portfolio.API.Services.Email;

public class GmailEmailService : IEmailService
{
    private readonly string _fromEmail;
    private readonly string _appPassword;
    private readonly string _toEmail;

    public GmailEmailService(IConfiguration configuration)
    {
        _fromEmail = configuration["Email:Gmail:FromEmail"]!;
        _appPassword = configuration["Email:Gmail:AppPassword"]!;
        _toEmail = configuration["Email:ToEmail"]!;
    }

    public async Task<bool> SendEmailAsync(string subject, string body)
    {
        try
        {
            using var client = new SmtpClient("smtp.gmail.com", 587)
            {
                Credentials = new NetworkCredential(_fromEmail, _appPassword),
                EnableSsl = true
            };

            var message = new MailMessage(_fromEmail, _toEmail, subject, body);
            await client.SendMailAsync(message);
            return true;
        }
        catch
        {
            return false;
        }
    }
}