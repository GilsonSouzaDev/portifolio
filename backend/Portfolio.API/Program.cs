using Microsoft.EntityFrameworkCore;
using Portfolio.API.Data;
using Portfolio.API.Services.Email;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseOracle(builder.Configuration.GetConnectionString("OracleDb")));

builder.Services.AddSingleton<GmailEmailService>();
builder.Services.AddSingleton<SendGridEmailService>();
builder.Services.AddSingleton<MailgunEmailService>();

builder.Services.AddSingleton<IEmailService>(provider => new FallbackEmailService(
    new IEmailService[]
    {
        provider.GetRequiredService<GmailEmailService>(),
        provider.GetRequiredService<SendGridEmailService>(),
        provider.GetRequiredService<MailgunEmailService>()
    },
    provider.GetRequiredService<ILogger<FallbackEmailService>>()
));

// Registrando as Camadas de Serviço (Clean Architecture / N-Tier)
builder.Services.AddScoped<Portfolio.API.Services.Interfaces.IProfileService, Portfolio.API.Services.Implementations.ProfileService>();
builder.Services.AddScoped<Portfolio.API.Services.Interfaces.ISkillsService, Portfolio.API.Services.Implementations.SkillsService>();
builder.Services.AddScoped<Portfolio.API.Services.Interfaces.IProjectsService, Portfolio.API.Services.Implementations.ProjectsService>();
builder.Services.AddScoped<Portfolio.API.Services.Interfaces.ISocialLinksService, Portfolio.API.Services.Implementations.SocialLinksService>();
builder.Services.AddScoped<Portfolio.API.Services.Interfaces.IContactService, Portfolio.API.Services.Implementations.ContactService>();


builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.WithOrigins("http://localhost:4200", "https://gilsonsouzadev.github.io")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

builder.Services.AddOpenApi();
builder.Services.AddControllers();
builder.Services.AddHttpClient();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    DataSeeder.Seed(db);
}

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseCors("AllowFrontend");
app.UseStaticFiles();
app.UseHttpsRedirection();
app.MapControllers();

app.Run();
[System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
public partial class Program { }
