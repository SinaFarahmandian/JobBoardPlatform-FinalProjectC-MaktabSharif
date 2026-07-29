using System.ComponentModel.DataAnnotations;

namespace JobBoardPlatform.Buisiness.Dtos.Email;

public class UpdateEmailTemplateDto
{
    [Required, StringLength(200)]
    public string Subject { get; set; } = string.Empty;

    [Required]
    public string Body { get; set; } = string.Empty;

    public bool IsEnabled { get; set; }
}