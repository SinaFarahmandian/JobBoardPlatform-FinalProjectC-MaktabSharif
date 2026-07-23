using JobBoardPlatform.Buisiness.Dtos.JobSeekerProfile;
using Microsoft.AspNetCore.Http;

namespace JobBoardPlatform.Buisiness.Interfaces;

public interface IJobSeekerProfileService
{
    Task<JobSeekerProfileDto> GetMyProfileAsync(int jobSeekerId);
    Task<JobSeekerProfileDto> UpdateMyProfileAsync(int jobSeekerId, UpdateJobSeekerProfileDto dto);
    Task<JobSeekerProfileDto> UploadResumeAsync(int jobSeekerId, IFormFile file);
    Task<JobSeekerProfileDto> DeleteResumeAsync(int jobSeekerId);
}