using JobBoardPlatform.Buisiness.Common.Exceptions;
using JobBoardPlatform.Buisiness.Dtos.JobSeekerProfile;
using JobBoardPlatform.Buisiness.Interfaces;
using Microsoft.AspNetCore.Http;

namespace JobBoardPlatform.Buisiness.Services;

public class JobSeekerProfileService : IJobSeekerProfileService
{
    private readonly IJobSeekerRepository _repo;

    private static readonly string[] AllowedExtensions = { ".pdf" };
    private const long MaxFileSizeBytes = 5 * 1024 * 1024; 

    public JobSeekerProfileService(IJobSeekerRepository repo)
    {
        _repo = repo;
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

        using var memoryStream = new MemoryStream();
        await file.CopyToAsync(memoryStream);

        seeker.ResumeData = memoryStream.ToArray();
        seeker.ResumeFileName = file.FileName;
        seeker.ResumeContentType = file.ContentType;
        seeker.UpdatedAt = DateTime.UtcNow;

        await _repo.UpdateAsync(seeker);

        return MapToDto(seeker);
    }

    public async Task<JobSeekerProfileDto> DeleteResumeAsync(int jobSeekerId)
    {
        var seeker = await GetOwnedAsync(jobSeekerId);

        seeker.ResumeData = null;
        seeker.ResumeFileName = null;
        seeker.ResumeContentType = null;
        seeker.UpdatedAt = DateTime.UtcNow;
        await _repo.UpdateAsync(seeker);

        return MapToDto(seeker);
    }
    
    public async Task<(byte[] Data, string ContentType, string FileName)> GetResumeAsync(int jobSeekerId)
    {
        var seeker = await GetOwnedAsync(jobSeekerId);

        if (seeker.ResumeData == null)
            throw new NotFoundException("رزومه‌ای برای این کارجو ثبت نشده است");

        return (seeker.ResumeData, seeker.ResumeContentType!, seeker.ResumeFileName!);
    }

    private async Task<Domain.Entities.JobSeekers.JobSeeker> GetOwnedAsync(int jobSeekerId)
        => await _repo.GetByIdAsync(jobSeekerId) ?? throw new NotFoundException("پروفایل کارجو پیدا نشد");

    private static JobSeekerProfileDto MapToDto(Domain.Entities.JobSeekers.JobSeeker s) => new()
    {
        Id = s.Id, FullName = s.FullName, Email = s.Email!, PhoneNumber = s.PhoneNumber,
        HasResume = s.ResumeData != null, ResumeFileName = s.ResumeFileName,
        Skills = s.Skills, YearsOfExperience = s.YearsOfExperience, DesiredJobTitle = s.DesiredJobTitle
    };
}