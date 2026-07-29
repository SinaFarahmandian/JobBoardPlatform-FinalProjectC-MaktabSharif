namespace JobBoardPlatform.Buisiness.Interfaces;

public interface IEmailService
{
    Task SendAsync(string toEmail, string subject, string body);
}