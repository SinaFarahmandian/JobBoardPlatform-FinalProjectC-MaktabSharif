namespace JobBoardPlatform.Buisiness.Interfaces;

public interface IEmailNotificationService
{
    Task SendAsync(string templateKey, string toEmail, Dictionary<string, string> placeholders);
}