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

builder.Services.AddOpenApi();
builder.Services.AddControllers();

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

app.UseHttpsRedirection();
app.MapControllers();

app.Run();