using JobBoardPlatform.Buisiness.Interfaces;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MimeKit;

namespace JobBoardPlatform.Infrastructure.Services;

public class MailKitEmailService : IEmailService
{
    private readonly IConfiguration _configuration;
    private readonly ILogger<MailKitEmailService> _logger;

    public MailKitEmailService(IConfiguration configuration, ILogger<MailKitEmailService> logger)
    {
        _configuration = configuration;
        _logger = logger;
    }

    public async Task SendAsync(string toEmail, string subject, string body)
    {
        var smtp = _configuration.GetSection("Smtp");
        var host = smtp["Host"];
        var fromEmail = smtp["FromEmail"] ?? string.Empty;

        if (string.IsNullOrWhiteSpace(host) || string.IsNullOrWhiteSpace(fromEmail))
        {
            _logger.LogWarning("SMTP settings are incomplete; the email to {Email} was not sent", toEmail);
            return;
        }

        if (!int.TryParse(smtp["Port"], out var port))
            port = 587;

        var message = new MimeMessage();
        message.From.Add(new MailboxAddress(smtp["FromName"], fromEmail));
        message.To.Add(MailboxAddress.Parse(toEmail));
        message.Subject = subject;
        message.Body = new TextPart("plain") { Text = body };

        using var client = new SmtpClient();
        await client.ConnectAsync(host, port, SecureSocketOptions.StartTls);

        var username = smtp["Username"];
        var password = smtp["Password"];
        if (!string.IsNullOrWhiteSpace(username) && !string.IsNullOrWhiteSpace(password))
            await client.AuthenticateAsync(username, password);

        await client.SendAsync(message);
        await client.DisconnectAsync(true);
    }
}
