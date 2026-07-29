using JobBoardPlatform.Buisiness.Interfaces;
using JobBoardPlatform.Domain.Entities.JobSeekers;
using JobBoardPlatform.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace JobBoardPlatform.Infrastructure.Repositories;

public class JobSeekerRepository : IJobSeekerRepository
{
    private readonly AppDbContext _context;
    
    public JobSeekerRepository(AppDbContext context) => _context = context;
    
    public async Task<List<JobSeeker>> GetAllAsync() =>
        await _context.Set<JobSeeker>().ToListAsync();

    public async Task<JobSeeker?> GetByIdAsync(int id) => await _context.Set<JobSeeker>().FindAsync(id);

    public async Task UpdateAsync(JobSeeker jobSeeker)
    {
        _context.Set<JobSeeker>().Update(jobSeeker);
        await _context.SaveChangesAsync();
    }
}