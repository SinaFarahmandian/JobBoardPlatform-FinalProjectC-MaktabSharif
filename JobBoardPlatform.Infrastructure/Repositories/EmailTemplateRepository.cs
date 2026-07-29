using JobBoardPlatform.Buisiness.Interfaces;
using JobBoardPlatform.Domain.Entities.Emails;
using JobBoardPlatform.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace JobBoardPlatform.Infrastructure.Repositories;

public class EmailTemplateRepository : IEmailTemplateRepository
{
    private readonly AppDbContext _context;
    public EmailTemplateRepository(AppDbContext context) => _context = context;

    public async Task<EmailTemplate?> GetByKeyAsync(string key) =>
        await _context.EmailTemplates.FirstOrDefaultAsync(t => t.Key == key);

    public async Task<List<EmailTemplate>> GetAllAsync() =>
        await _context.EmailTemplates.ToListAsync();

    public async Task UpdateAsync(EmailTemplate template)
    {
        _context.EmailTemplates.Update(template);
        await _context.SaveChangesAsync();
    }
}