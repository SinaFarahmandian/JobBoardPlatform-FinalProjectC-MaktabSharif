using JobBoardPlatform.Domain.Entities.JobApplications;
using JobBoardPlatform.Domain.Enums;

namespace JobBoardPlatform.Domain.Entities.JobSeekers;

public class JobSeeker : User
{
    public byte[]? ResumeData { get; set; }
    public string? ResumeFileName { get; set; }
    public string? ResumeContentType { get; set; }
    public string? Skills { get; set; }
    public int YearsOfExperience { get; set; }
    public string? DesiredJobTitle { get; set; }

    public ICollection<JobApplication> Applications { get; set; } = new List<JobApplication>();

    private JobSeeker() { }

    public JobSeeker(string fullName, string email) : base(fullName, email)
    {
    }
}