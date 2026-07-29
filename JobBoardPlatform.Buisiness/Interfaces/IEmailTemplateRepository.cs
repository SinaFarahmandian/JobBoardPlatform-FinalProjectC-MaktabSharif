using JobBoardPlatform.Domain.Entities.Emails;

namespace JobBoardPlatform.Buisiness.Interfaces;

public interface IEmailTemplateRepository
{
    Task<EmailTemplate?> GetByKeyAsync(string key);
    Task<List<EmailTemplate>> GetAllAsync();
    Task UpdateAsync(EmailTemplate template);
}