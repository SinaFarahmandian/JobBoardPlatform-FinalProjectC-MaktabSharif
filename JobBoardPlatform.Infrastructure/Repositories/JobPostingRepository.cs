using JobBoardPlatform.Buisiness.Interfaces;
using JobBoardPlatform.Domain.Entities.JobPostings;
using JobBoardPlatform.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace JobBoardPlatform.Infrastructure.Repositories;

public class JobPostingRepository : IJobPostingRepository
{
    private readonly AppDbContext _context;
    public JobPostingRepository(AppDbContext context) => _context = context;

    public async Task<JobPosting?> GetByIdAsync(int id) =>
        await _context.JobPostings.Include(jp => jp.Employer).FirstOrDefaultAsync(jp => jp.Id == id);

    public async Task<List<JobPosting>> GetByEmployerIdAsync(int employerId) =>
        await _context.JobPostings.Where(jp => jp.EmployerId == employerId).ToListAsync();

    public async Task AddAsync(JobPosting posting)
    {
        await _context.JobPostings.AddAsync(posting);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(JobPosting posting)
    {
        _context.JobPostings.Update(posting);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(JobPosting posting)
    {
        posting.IsDeleted = true; 
        _context.JobPostings.Update(posting);
        await _context.SaveChangesAsync();
    }
}