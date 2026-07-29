using JobBoardPlatform.Buisiness.Interfaces;
using JobBoardPlatform.Domain.Entities.Employers;
using JobBoardPlatform.Domain.Entities.JobSeekers;
using JobBoardPlatform.Domain.Enums;
using JobBoardPlatform.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace JobBoardPlatform.Infrastructure.Repositories;

public class DashboardRepository : IDashboardRepository
{
    private readonly AppDbContext _context;
    public DashboardRepository(AppDbContext context) => _context = context;

    public async Task<int> GetJobSeekerCountAsync() => await _context.Set<JobSeeker>().CountAsync();
    public async Task<int> GetEmployerCountAsync() => await _context.Set<Employer>().CountAsync();
    public async Task<int> GetActiveJobPostingCountAsync() => await _context.JobPostings.CountAsync(jp => jp.IsActive);
    public async Task<int> GetInactiveJobPostingCountAsync() => await _context.JobPostings.CountAsync(jp => !jp.IsActive);

    public async Task<Dictionary<ApplicationStatus, int>> GetApplicationCountsByStatusAsync() =>
        await _context.JobApplications
            .GroupBy(ja => ja.Status)
            .Select(g => new { Status = g.Key, Count = g.Count() })
            .ToDictionaryAsync(x => x.Status, x => x.Count);

    public async Task<int> GetPendingEmployerCountAsync() =>
        await _context.Set<Employer>().CountAsync(e => !e.IsApproved);
}