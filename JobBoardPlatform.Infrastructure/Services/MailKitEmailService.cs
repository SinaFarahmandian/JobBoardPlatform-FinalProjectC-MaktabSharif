using JobBoardPlatform.Buisiness.Interfaces;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Configuration;
using MimeKit;

namespace JobBoardPlatform.Infrastructure.Services;

public class MailKitEmailService : IEmailService
{
    private readonly IConfiguration _configuration;
    public MailKitEmailService(IConfiguration configuration) => _configuration = configuration;

    public async Task SendAsync(string toEmail, string subject, string body)
    {
        var smtp = _configuration.GetSection("Smtp");

        var message = new MimeMessage();
        message.From.Add(new MailboxAddress(smtp["FromName"], smtp["FromEmail"]));
        message.To.Add(MailboxAddress.Parse(toEmail));
        message.Subject = subject;
        message.Body = new TextPart("plain") { Text = body };

        using var client = new SmtpClient();
        await client.ConnectAsync(smtp["Host"], int.Parse(smtp["Port"]!), SecureSocketOptions.StartTls);
        await client.AuthenticateAsync(smtp["Username"], smtp["Password"]);
        await client.SendAsync(message);
        await client.DisconnectAsync(true);
    }
}