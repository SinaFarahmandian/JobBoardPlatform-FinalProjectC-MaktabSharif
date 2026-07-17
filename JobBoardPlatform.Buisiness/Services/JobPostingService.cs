using JobBoardPlatform.Buisiness.Common.Exceptions;
using JobBoardPlatform.Buisiness.Dtos.JobPosting;
using JobBoardPlatform.Buisiness.Interfaces;
using JobBoardPlatform.Domain.Entities.JobPostings;

namespace JobBoardPlatform.Buisiness.Services;

public class JobPostingService : IJobPostingService
{
    private readonly IJobPostingRepository _repo;

    public JobPostingService(IJobPostingRepository repo) => _repo = repo;

    public async Task<JobPostingDto> CreateAsync(int employerId, CreateJobPostingDto dto)
    {
        var posting = new JobPosting(dto.Title, dto.Description, dto.Location, employerId, dto.EmploymentType)
        {
            SalaryMin = dto.SalaryMin,
            SalaryMax = dto.SalaryMax,
            ExpiresAt = dto.ExpiresAt
        };

        await _repo.AddAsync(posting);
        return MapToDto(posting);
    }

    public async Task<JobPostingDto> UpdateAsync(int employerId, int jobPostingId, UpdateJobPostingDto dto)
    {
        var posting = await GetOwnedAsync(employerId, jobPostingId);

        posting.Title = dto.Title;
        posting.Description = dto.Description;
        posting.Location = dto.Location;
        posting.SalaryMin = dto.SalaryMin;
        posting.SalaryMax = dto.SalaryMax;
        posting.EmploymentType = dto.EmploymentType;
        posting.ExpiresAt = dto.ExpiresAt;
        posting.UpdatedAt = DateTime.UtcNow;

        await _repo.UpdateAsync(posting);
        return MapToDto(posting);
    }

    public async Task DeleteAsync(int employerId, int jobPostingId)
    {
        var posting = await GetOwnedAsync(employerId, jobPostingId);
        await _repo.DeleteAsync(posting);
    }

    public async Task<List<JobPostingDto>> GetMyJobPostingsAsync(int employerId)
    {
        var postings = await _repo.GetByEmployerIdAsync(employerId);
        return postings.Select(MapToDto).ToList();
    }

    public async Task<JobPostingDto> GetByIdAsync(int employerId, int jobPostingId)
        => MapToDto(await GetOwnedAsync(employerId, jobPostingId));

    public async Task<JobPostingDto> ToggleActiveAsync(int employerId, int jobPostingId)
    {
        var posting = await GetOwnedAsync(employerId, jobPostingId);
        posting.IsActive = !posting.IsActive;
        posting.UpdatedAt = DateTime.UtcNow;
        await _repo.UpdateAsync(posting);
        return MapToDto(posting);
    }

    private async Task<JobPosting> GetOwnedAsync(int employerId, int jobPostingId)
    {
        var posting = await _repo.GetByIdAsync(jobPostingId)
            ?? throw new NotFoundException("آگهی پیدا نشد");

        if (posting.EmployerId != employerId)
            throw new ForbiddenAccessException("شما به این آگهی دسترسی ندارید");

        return posting;
    }

    private static JobPostingDto MapToDto(JobPosting p) => new()
    {
        Id = p.Id, Title = p.Title, Description = p.Description, Location = p.Location,
        SalaryMin = p.SalaryMin, SalaryMax = p.SalaryMax, EmploymentType = p.EmploymentType,
        ExpiresAt = p.ExpiresAt, IsFeatured = p.IsFeatured, IsActive = p.IsActive, CreatedAt = p.CreatedAt
    };
}