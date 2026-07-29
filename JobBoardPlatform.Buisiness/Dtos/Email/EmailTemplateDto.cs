namespace JobBoardPlatform.Buisiness.Dtos.Email;

public class EmailTemplateDto
{
    public string Key { get; set; } = string.Empty;
    public string Subject { get; set; } = string.Empty;
    public string Body { get; set; } = string.Empty;
    public bool IsEnabled { get; set; }
}