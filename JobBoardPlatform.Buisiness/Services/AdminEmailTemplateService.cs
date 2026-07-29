using JobBoardPlatform.Buisiness.Common.Exceptions;
using JobBoardPlatform.Buisiness.Dtos.Email;
using JobBoardPlatform.Buisiness.Interfaces;
using JobBoardPlatform.Domain.Entities.Emails;

namespace JobBoardPlatform.Buisiness.Services;

public class AdminEmailTemplateService : IAdminEmailTemplateService
{
    private readonly IEmailTemplateRepository _repo;
    public AdminEmailTemplateService(IEmailTemplateRepository repo) => _repo = repo;

    public async Task<List<EmailTemplateDto>> GetAllAsync()
    {
        var templates = await _repo.GetAllAsync();
        return templates.Select(MapToDto).ToList();
    }

    public async Task<EmailTemplateDto> UpdateAsync(string key, UpdateEmailTemplateDto dto)
    {
        var template = await _repo.GetByKeyAsync(key) ?? throw new NotFoundException("قالب ایمیل پیدا نشد");
        template.Subject = dto.Subject;
        template.Body = dto.Body;
        template.IsEnabled = dto.IsEnabled;
        template.UpdatedAt = DateTime.UtcNow;
        await _repo.UpdateAsync(template);
        return MapToDto(template);
    }

    private static EmailTemplateDto MapToDto(EmailTemplate t) => new()
    {
        Key = t.Key, Subject = t.Subject, Body = t.Body, IsEnabled = t.IsEnabled
    };
}