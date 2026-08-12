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

    public async Task<JobSeekerProfileDto> UploadResumeAsync(int jobSeekerId, IFormFile? file)
    {
        if (file == null || file.Length == 0)
            throw new BadRequestException("No file was uploaded");

        if (file.Length > MaxFileSizeBytes)
            throw new BadRequestException("The file size must not exceed 5 MB");

        var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
        if (!AllowedExtensions.Contains(extension))
            throw new BadRequestException("Only PDF files are allowed");

        var seeker = await GetOwnedAsync(jobSeekerId);

        using var memoryStream = new MemoryStream();
        await file.CopyToAsync(memoryStream);

        var resumeData = memoryStream.ToArray();
        if (resumeData.Length < 5 || resumeData[0] != (byte)'%' || resumeData[1] != (byte)'P'
            || resumeData[2] != (byte)'D' || resumeData[3] != (byte)'F' || resumeData[4] != (byte)'-')
            throw new BadRequestException("The uploaded file is not a valid PDF");

        seeker.ResumeData = resumeData;
        seeker.ResumeFileName = Path.GetFileName(file.FileName);
        seeker.ResumeContentType = "application/pdf";
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
            throw new NotFoundException("No resume has been uploaded for this job seeker");

        return (seeker.ResumeData, seeker.ResumeContentType!, seeker.ResumeFileName!);
    }

    private async Task<Domain.Entities.JobSeekers.JobSeeker> GetOwnedAsync(int jobSeekerId)
        => await _repo.GetByIdAsync(jobSeekerId) ?? throw new NotFoundException("The job seeker profile was not found");

    private static JobSeekerProfileDto MapToDto(Domain.Entities.JobSeekers.JobSeeker s) => new()
    {
        Id = s.Id, FullName = s.FullName, Email = s.Email!, PhoneNumber = s.PhoneNumber,
        HasResume = s.ResumeData != null, ResumeFileName = s.ResumeFileName,
        Skills = s.Skills, YearsOfExperience = s.YearsOfExperience, DesiredJobTitle = s.DesiredJobTitle
    };
}
