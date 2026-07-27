using JobBoardPlatform.Buisiness.Interfaces;
using JobBoardPlatform.Domain.Entities.JobApplications;
using JobBoardPlatform.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace JobBoardPlatform.Infrastructure.Repositories;

public class JobApplicationRepository : IJobApplicationRepository
{
    private readonly AppDbContext _context;
    public JobApplicationRepository(AppDbContext context) => _context = context;

    public async Task<JobApplication?> GetByIdAsync(int id) =>
        await _context.JobApplications
            .Include(ja => ja.JobPosting).ThenInclude(jp => jp.Employer).ThenInclude(e => e.Company)
            .Include(ja => ja.JobSeeker)
            .FirstOrDefaultAsync(ja => ja.Id == id);

    public async Task<List<JobApplication>> GetByJobPostingIdAsync(int jobPostingId) =>
        await _context.JobApplications
            .Include(ja => ja.JobSeeker)
            .Where(ja => ja.JobPostingId == jobPostingId)
            .ToListAsync();

    public async Task<List<JobApplication>> GetByJobSeekerIdAsync(int jobSeekerId) =>
        await _context.JobApplications
            .Include(ja => ja.JobPosting).ThenInclude(jp => jp.Employer).ThenInclude(e => e.Company)
            .Where(ja => ja.JobSeekerId == jobSeekerId)
            .OrderByDescending(ja => ja.CreatedAt)
            .ToListAsync();

    public async Task<bool> ExistsAsync(int jobSeekerId, int jobPostingId) =>
        await _context.JobApplications.AnyAsync(ja => ja.JobSeekerId == jobSeekerId && ja.JobPostingId == jobPostingId);

    public async Task AddAsync(JobApplication application)
    {
        await _context.JobApplications.AddAsync(application);
        await _context.SaveChangesAsync();
    }
    
    public async Task UpdateAsync(JobApplication application)
    {
        _context.JobApplications.Update(application);
        await _context.SaveChangesAsync();
    }
}