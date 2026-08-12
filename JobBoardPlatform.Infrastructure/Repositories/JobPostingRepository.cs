using JobBoardPlatform.Buisiness.Dtos.JobPosting;
using JobBoardPlatform.Buisiness.Interfaces;
using JobBoardPlatform.Domain.Entities.JobPostings;
using JobBoardPlatform.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace JobBoardPlatform.Infrastructure.Repositories;

public class JobPostingRepository : IJobPostingRepository
{
    private readonly AppDbContext _context;
    public JobPostingRepository(AppDbContext context) => _context = context;
    
    public async Task<List<JobPosting>> GetAllForAdminAsync() =>
        await _context.JobPostings
            .Include(jp => jp.Employer).ThenInclude(e => e.Company)
            .OrderByDescending(jp => jp.CreatedAt)
            .ToListAsync();

    public async Task<JobPosting?> GetByIdAsync(int id) =>
        await _context.JobPostings.Include(jp => jp.Employer).FirstOrDefaultAsync(jp => jp.Id == id);

    public async Task<List<JobPosting>> GetByEmployerIdAsync(int employerId) =>
        await _context.JobPostings.Where(jp => jp.EmployerId == employerId).ToListAsync();

    
    public async Task<(List<JobPosting> Items, int TotalCount)> SearchActiveAsync(JobPostingSearchQueryDto query)
    {
        var q = _context.JobPostings
            .Include(jp => jp.Employer).ThenInclude(e => e.Company)
            .Where(jp => jp.IsActive
                         && (jp.ExpiresAt == null || jp.ExpiresAt > DateTime.UtcNow)
                         && jp.Employer.IsActive
                         && jp.Employer.IsApproved);

        if (!string.IsNullOrWhiteSpace(query.Search))
            q = q.Where(jp => jp.Title.Contains(query.Search));

        if (!string.IsNullOrWhiteSpace(query.EmploymentType))
            q = q.Where(jp => jp.EmploymentType == query.EmploymentType);

        if (!string.IsNullOrWhiteSpace(query.City))
            q = q.Where(jp => jp.Location.Contains(query.City));

        if (!string.IsNullOrWhiteSpace(query.Category))
            q = q.Where(jp => jp.Category == query.Category);

        if (query.MinSalary.HasValue)
            q = q.Where(jp => jp.SalaryMax == null || jp.SalaryMax >= query.MinSalary);

        if (query.MaxSalary.HasValue)
            q = q.Where(jp => jp.SalaryMin == null || jp.SalaryMin <= query.MaxSalary);

        if (!string.IsNullOrWhiteSpace(query.Skill))
            q = q.Where(jp => jp.Skills != null && jp.Skills.Contains(query.Skill));

        var totalCount = await q.CountAsync();

        var items = await q
            .OrderByDescending(jp => jp.IsFeatured && (jp.FeaturedUntil == null || jp.FeaturedUntil > DateTime.UtcNow))
            .ThenByDescending(jp => jp.CreatedAt)
            .Skip((query.Page - 1) * query.PageSize)
            .Take(query.PageSize)
            .ToListAsync();

        return (items, totalCount);
    }

    public async Task<JobPosting?> GetActiveByIdAsync(int id) =>
        await _context.JobPostings
            .Include(jp => jp.Employer).ThenInclude(e => e.Company)
            .FirstOrDefaultAsync(jp => jp.Id == id
                                       && jp.IsActive
                                       && (jp.ExpiresAt == null || jp.ExpiresAt > DateTime.UtcNow)
                                       && jp.Employer.IsActive
                                       && jp.Employer.IsApproved);
    
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