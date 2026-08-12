using JobBoardPlatform.Buisiness.Common.Exceptions;
using JobBoardPlatform.Buisiness.Dtos.Admin;
using JobBoardPlatform.Buisiness.Interfaces;
using JobBoardPlatform.Domain.Entities.JobPostings;

namespace JobBoardPlatform.Buisiness.Services;

public class AdminJobPostingService : IAdminJobPostingService
{
    private readonly IJobPostingRepository _repo;
    public AdminJobPostingService(IJobPostingRepository repo) => _repo = repo;

    public async Task<List<JobPostingAdminDto>> GetAllAsync()
    {
        var postings = await _repo.GetAllForAdminAsync();
        return postings.Select(MapToDto).ToList();
    }

    public async Task SetActiveStatusAsync(int jobPostingId, bool isActive)
    {
        var posting = await _repo.GetByIdAsync(jobPostingId) ?? throw new NotFoundException("The job posting was not found");
        posting.IsActive = isActive;
        posting.UpdatedAt = DateTime.UtcNow;
        await _repo.UpdateAsync(posting);
    }

    public async Task DeleteAsync(int jobPostingId)
    {
        var posting = await _repo.GetByIdAsync(jobPostingId) ?? throw new NotFoundException("The job posting was not found");
        await _repo.DeleteAsync(posting);  
    }

    public async Task SetFeaturedAsync(int jobPostingId, SetFeaturedDto dto)
    {
        var posting = await _repo.GetByIdAsync(jobPostingId) ?? throw new NotFoundException("The job posting was not found");
        posting.IsFeatured = dto.IsFeatured;
        posting.FeaturedUntil = dto.IsFeatured ? dto.FeaturedUntil : null;
        posting.UpdatedAt = DateTime.UtcNow;
        await _repo.UpdateAsync(posting);
    }

    private static JobPostingAdminDto MapToDto(JobPosting p) => new()
    {
        Id = p.Id, Title = p.Title, CompanyName = p.Employer?.Company?.Name ?? "",
        EmployerFullName = p.Employer?.FullName ?? "", IsActive = p.IsActive,
        IsFeatured = p.IsFeatured, FeaturedUntil = p.FeaturedUntil, CreatedAt = p.CreatedAt
    };
}
