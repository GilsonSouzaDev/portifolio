using System.Net.Http.Headers;
using System.Text;

namespace Portfolio.API.Services.Email;

public class MailgunEmailService : IEmailService
{
    private readonly string _apiKey;
    private readonly string _domain;
    private readonly string _fromEmail;
    private readonly string _toEmail;

    public MailgunEmailService(IConfiguration configuration)
    {
        _apiKey = configuration["Email:Mailgun:ApiKey"]!;
        _domain = configuration["Email:Mailgun:Domain"]!;
        _fromEmail = configuration["Email:Mailgun:FromEmail"]!;
        _toEmail = configuration["Email:ToEmail"]!;
    }

    public async Task<bool> SendEmailAsync(string subject, string body)
    {
        try
        {
            using var client = new HttpClient();
            var credentials = Convert.ToBase64String(Encoding.ASCII.GetBytes($"api:{_apiKey}"));
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", credentials);

            var form = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("from", _fromEmail),
                new KeyValuePair<string, string>("to", _toEmail),
                new KeyValuePair<string, string>("subject", subject),
                new KeyValuePair<string, string>("text", body)
            });

            var response = await client.PostAsync($"https://api.mailgun.net/v3/{_domain}/messages", form);
            return response.IsSuccessStatusCode;
        }
        catch
        {
            return false;
        }
    }
}