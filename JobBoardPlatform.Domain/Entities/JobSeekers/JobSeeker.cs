using JobBoardPlatform.Domain.Entities.JobApplications;
using JobBoardPlatform.Domain.Enums;

namespace JobBoardPlatform.Domain.Entities.JobSeekers;

public class JobSeeker : User
{
    public JobSeeker() => Role = UserRole.JobSeeker;

    public string? ResumeUrl { get; set; }
    public string? Skills { get; set; }          
    public string? YearsOfExperience { get; set; }

    public List<JobApplication> Applications { get; set; } = new();
}