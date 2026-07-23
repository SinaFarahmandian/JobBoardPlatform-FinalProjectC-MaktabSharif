using JobBoardPlatform.Buisiness.Common.Exceptions;
using JobBoardPlatform.Buisiness.Dtos.JobSeekerProfile;
using JobBoardPlatform.Buisiness.Interfaces;
using Microsoft.AspNetCore.Http;

namespace JobBoardPlatform.Buisiness.Services;

public class JobSeekerProfileService : IJobSeekerProfileService
{
    private readonly IJobSeekerRepository _repo;
    private readonly IFileStorageService _fileStorage;

    private static readonly string[] AllowedExtensions = { ".pdf" };
    private const long MaxFileSizeBytes = 5 * 1024 * 1024; 

    public JobSeekerProfileService(IJobSeekerRepository repo, IFileStorageService fileStorage)
    {
        _repo = repo;
        _fileStorage = fileStorage;
    }

    public async Task<JobSeekerProfileDto> GetMyProfileAsync(int jobSeekerId)
        => MapToDto(await GetOwnedAsync(jobSeekerId));

    public async Task<JobSeekerProfileDto> UpdateMyProfileAsync(int jobSeekerId, UpdateJobSeekerProfileDto dto)
    {
        var seeker = await GetOwnedAsync(jobSeekerId);

        seeker.FullName = dto.FullName;
        seeker.PhoneNumber = dto.PhoneNumber;
        seeker.Skills = dto.Skills;
        seeker.YearsOfExperience = dto.YearsOfExperience;
        seeker.DesiredJobTitle = dto.DesiredJobTitle;
        seeker.UpdatedAt = DateTime.UtcNow;

        await _repo.UpdateAsync(seeker);
        return MapToDto(seeker);
    }

    public async Task<JobSeekerProfileDto> UploadResumeAsync(int jobSeekerId, IFormFile file)
    {
        if (file == null || file.Length == 0)
            throw new BadRequestException("فایلی ارسال نشده است");

        if (file.Length > MaxFileSizeBytes)
            throw new BadRequestException("حجم فایل نباید بیشتر از ۵ مگابایت باشد");

        var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
        if (!AllowedExtensions.Contains(extension))
            throw new BadRequestException("فقط فایل PDF مجاز است");

        var seeker = await GetOwnedAsync(jobSeekerId);

        if (!string.IsNullOrEmpty(seeker.ResumeUrl))
            _fileStorage.DeleteResume(seeker.ResumeUrl);

        await using var stream = file.OpenReadStream();
        var storedPath = await _fileStorage.SaveResumeAsync(jobSeekerId, stream, file.FileName);

        seeker.ResumeUrl = storedPath;
        seeker.UpdatedAt = DateTime.UtcNow;
        await _repo.UpdateAsync(seeker);

        return MapToDto(seeker);
    }

    public async Task<JobSeekerProfileDto> DeleteResumeAsync(int jobSeekerId)
    {
        var seeker = await GetOwnedAsync(jobSeekerId);

        if (!string.IsNullOrEmpty(seeker.ResumeUrl))
        {
            _fileStorage.DeleteResume(seeker.ResumeUrl);
            seeker.ResumeUrl = null;
            seeker.UpdatedAt = DateTime.UtcNow;
            await _repo.UpdateAsync(seeker);
        }

        return MapToDto(seeker);
    }

    private async Task<Domain.Entities.JobSeekers.JobSeeker> GetOwnedAsync(int jobSeekerId)
        => await _repo.GetByIdAsync(jobSeekerId) ?? throw new NotFoundException("پروفایل کارجو پیدا نشد");

    private static JobSeekerProfileDto MapToDto(Domain.Entities.JobSeekers.JobSeeker s) => new()
    {
        Id = s.Id, FullName = s.FullName, Email = s.Email!, PhoneNumber = s.PhoneNumber,
        ResumeUrl = s.ResumeUrl, Skills = s.Skills, YearsOfExperience = s.YearsOfExperience,
        DesiredJobTitle = s.DesiredJobTitle
    };
}