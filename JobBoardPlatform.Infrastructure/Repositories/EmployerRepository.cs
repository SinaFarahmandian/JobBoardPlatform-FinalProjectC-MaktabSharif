using JobBoardPlatform.Buisiness.Interfaces;
using JobBoardPlatform.Domain.Entities.Employers;
using JobBoardPlatform.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace JobBoardPlatform.Infrastructure.Repositories;

public class EmployerRepository : IEmployerRepository
{
    private readonly AppDbContext _context;
    
    public EmployerRepository(AppDbContext context) => _context = context;
    
    public async Task<List<Employer>> GetAllAsync() =>
        await _context.Set<Employer>().Include(e => e.Company).ToListAsync();

    public async Task<Employer?> GetByIdAsync(int id) => await _context.Set<Employer>().FindAsync(id);

    public async Task<Employer?> GetByIdWithCompanyAsync(int id) =>
        await _context.Set<Employer>().Include(e => e.Company).FirstOrDefaultAsync(e => e.Id == id);

    public async Task<List<Employer>> GetPendingApprovalAsync() =>
        await _context.Set<Employer>().Include(e => e.Company)
            .Where(e => !e.IsApproved && e.IsActive)
            .ToListAsync();

    public async Task UpdateAsync(Employer employer)
    {
        _context.Set<Employer>().Update(employer);
        await _context.SaveChangesAsync();
    }
}