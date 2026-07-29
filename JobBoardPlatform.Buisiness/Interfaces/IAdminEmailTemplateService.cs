using JobBoardPlatform.Buisiness.Dtos.Email;

namespace JobBoardPlatform.Buisiness.Interfaces;

public interface IAdminEmailTemplateService
{
    Task<List<EmailTemplateDto>> GetAllAsync();
    Task<EmailTemplateDto> UpdateAsync(string key, UpdateEmailTemplateDto dto);
}