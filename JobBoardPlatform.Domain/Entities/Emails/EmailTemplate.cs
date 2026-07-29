namespace JobBoardPlatform.Domain.Entities.Emails;

public class EmailTemplate : BaseEntity
{
    public string Key { get; set; } = string.Empty;      
    public string Subject { get; set; } = string.Empty;
    public string Body { get; set; } = string.Empty;
    public bool IsEnabled { get; set; } = true;

    private EmailTemplate() { }

    public EmailTemplate(string key, string subject, string body)
    {
        Key = key;
        Subject = subject;
        Body = body;
    }
}