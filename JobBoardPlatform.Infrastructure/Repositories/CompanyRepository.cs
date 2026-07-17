using JobBoardPlatform.Buisiness.Interfaces;
using JobBoardPlatform.Domain.Entities.Companies;
using JobBoardPlatform.Infrastructure.Data;

namespace JobBoardPlatform.Infrastructure.Repositories;

public class CompanyRepository : ICompanyRepository
{
    private readonly AppDbContext _context;
    public CompanyRepository(AppDbContext context) => _context = context;

    public async Task<Company?> GetByIdAsync(int id) => await _context.Companies.FindAsync(id);

    public async Task AddAsync(Company company)
    {
        await _context.Companies.AddAsync(company);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Company company)
    {
        _context.Companies.Update(company);
        await _context.SaveChangesAsync();
    }
}