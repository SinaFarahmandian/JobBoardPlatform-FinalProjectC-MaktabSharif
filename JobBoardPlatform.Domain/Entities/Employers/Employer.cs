using JobBoardPlatform.Domain.Entities.Companies;
using JobBoardPlatform.Domain.Entities.JobPostings;
using JobBoardPlatform.Domain.Enums;

namespace JobBoardPlatform.Domain.Entities.Employers;

public class Employer : User
{
    public int CompanyId { get; set; }
    public Company Company { get; set; } = null!;

    public ICollection<JobPosting> JobPostings { get; set; } = new List<JobPosting>();

    private Employer() { }

    public Employer(string fullName, string email, int companyId) : base(fullName, email)
    {
        CompanyId = companyId;
        IsApproved = false; 
    }
}