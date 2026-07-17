using JobBoardPlatform.Domain.Entities.JobApplications;
using JobBoardPlatform.Domain.Enums;

namespace JobBoardPlatform.Domain.Entities.JobSeekers;

// JobSeeker.cs
public class JobSeeker : User
{
    public string? ResumeUrl { get; set; }
    public string? Skills { get; set; }
    public int YearsOfExperience { get; set; }
    public string? DesiredJobTitle { get; set; }

    public ICollection<JobApplication> Applications { get; set; } = new List<JobApplication>();

    private JobSeeker() { }

    public JobSeeker(string fullName, string email) : base(fullName, email)
    {
        Role = UserRole.JobSeeker;
    }
}