using JobBoardPlatform.Buisiness.Interfaces;
using Microsoft.Extensions.Logging;

namespace JobBoardPlatform.Buisiness.Services;

public class EmailNotificationService : IEmailNotificationService
{
    private readonly IEmailTemplateRepository _templateRepo;
    private readonly IEmailService _emailService;
    private readonly ILogger<EmailNotificationService> _logger;

    public EmailNotificationService(
        IEmailTemplateRepository templateRepo,
        IEmailService emailService,
        ILogger<EmailNotificationService> logger)
    {
        _templateRepo = templateRepo;
        _emailService = emailService;
        _logger = logger;
    }

    public async Task SendAsync(string templateKey, string toEmail, Dictionary<string, string> placeholders)
    {
        var template = await _templateRepo.GetByKeyAsync(templateKey);

        if (template == null || !template.IsEnabled)
            return;

        var subject = ReplacePlaceholders(template.Subject, placeholders);
        var body = ReplacePlaceholders(template.Body, placeholders);

        try
        {
            await _emailService.SendAsync(toEmail, subject, body);
        }
        catch (Exception ex)
        {
            // An email service failure must not fail the primary business operation.
            _logger.LogError(ex, "Sending the {TemplateKey} email template to {Email} failed", templateKey, toEmail);
        }
    }

    private static string ReplacePlaceholders(string text, Dictionary<string, string> placeholders)
    {
        foreach (var kv in placeholders)
            text = text.Replace($"{{{kv.Key}}}", kv.Value);
        return text;
    }
}
