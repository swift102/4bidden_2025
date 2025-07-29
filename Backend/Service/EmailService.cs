using System.Net.Mail;
using System.Net;
namespace Backend.Service;

public interface IEmailService
{
    Task SendWelcomeEmail(string email, string password);
}

public class EmailService : IEmailService
{
    private readonly IConfiguration _config;

    public EmailService(IConfiguration config)
    {
        _config = config;
    }

    public async Task SendWelcomeEmail(string email, string password)
    {
        var smtpServer = _config["EmailSettings:SMTPServer"];
        var port = int.Parse(_config["EmailSettings:Port"]);
        var login = _config["EmailSettings:Login"];
        var passwordConfig = _config["EmailSettings:Password"];
        var fromEmail = _config["EmailSettings:FromEmail"];

        var client = new SmtpClient(smtpServer, port)
        {
            Credentials = new NetworkCredential(login, passwordConfig),
            EnableSsl = true
        };

        var message = new MailMessage(fromEmail, email)
        {
            Subject = "Welcome to our system",
            Body = $"Welcome! Your account has been created. Your password is: {password}"
        };

        await client.SendMailAsync(message);
    }
}

