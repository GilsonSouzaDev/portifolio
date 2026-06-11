using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace Portfolio.API.Services.Email;

public class SendGridEmailService : IEmailService
{
    private readonly string _apiKey;
    private readonly string _fromEmail;
    private readonly string _toEmail;

    public SendGridEmailService(IConfiguration configuration)
    {
        _apiKey = configuration["Email:SendGrid:ApiKey"]!;
        _fromEmail = configuration["Email:SendGrid:FromEmail"]!;
        _toEmail = configuration["Email:ToEmail"]!;
    }

    public async Task<bool> SendEmailAsync(string subject, string body)
    {
        try
        {
            using var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _apiKey);

            var payload = new
            {
                personalizations = new[] { new { to = new[] { new { email = _toEmail } } } },
                from = new { email = _fromEmail },
                subject,
                content = new[] { new { type = "text/plain", value = body } }
            };

            var json = JsonSerializer.Serialize(payload);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await client.PostAsync("https://api.sendgrid.com/v3/mail/send", content);
            return response.IsSuccessStatusCode;
        }
        catch
        {
            return false;
        }
    }
}