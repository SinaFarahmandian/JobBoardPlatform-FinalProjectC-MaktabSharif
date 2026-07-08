using JobBoardPlatform.Domain.Entities.JobPostings;
using JobBoardPlatform.Domain.Enums;

namespace JobBoardPlatform.Domain.Entities.Employers;

public class Employer : User
{
    public Employer() => Role = UserRole.Employer;

    public string CompanyName { get; set; } = string.Empty;
    public string? CompanyWebsite { get; set; }
    public string? CompanyDescription { get; set; }

    public List<JobPosting> JobPostings { get; set; } = new();
}