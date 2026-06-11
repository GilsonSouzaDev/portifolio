namespace Portfolio.API.Services.Email;

public class FallbackEmailService : IEmailService
{
    private readonly IEnumerable<IEmailService> _services;
    private readonly ILogger<FallbackEmailService> _logger;

    public FallbackEmailService(IEnumerable<IEmailService> services, ILogger<FallbackEmailService> logger)
    {
        _services = services;
        _logger = logger;
    }

    public async Task<bool> SendEmailAsync(string subject, string body)
    {
        foreach (var service in _services)
        {
            var success = await service.SendEmailAsync(subject, body);
            if (success)
            {
                _logger.LogInformation("E-mail enviado com sucesso via {Service}", service.GetType().Name);
                return true;
            }
            _logger.LogWarning("Falha ao enviar e-mail via {Service}. Tentando próximo...", service.GetType().Name);
        }

        _logger.LogError("Todos os provedores de e-mail falharam.");
        return false;
    }
}