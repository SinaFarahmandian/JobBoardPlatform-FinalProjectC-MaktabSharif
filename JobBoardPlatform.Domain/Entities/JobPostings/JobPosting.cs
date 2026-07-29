using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using JobBoardPlatform.Domain.Entities.Employers;
using JobBoardPlatform.Domain.Entities.JobApplications;

namespace JobBoardPlatform.Domain.Entities.JobPostings;

public class JobPosting : BaseEntity
{
    [Required, StringLength(150, MinimumLength = 3)]
    public string Title { get; set; } = string.Empty;

    [Required, StringLength(5000, MinimumLength = 20)]
    public string Description { get; set; } = string.Empty;

    [Required, StringLength(100)]
    public string Location { get; set; } = string.Empty;

    [Column(TypeName = "decimal(18,2)")]
    [Range(0, double.MaxValue)]
    public decimal? SalaryMin { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    [Range(0, double.MaxValue)]
    public decimal? SalaryMax { get; set; }

    [Required, StringLength(50)]
    public string EmploymentType { get; set; } = "FullTime";
    
    [StringLength(100)]
    public string? Category { get; set; }

    [StringLength(500)]
    public string? Skills { get; set; }

    public DateTime? ExpiresAt { get; set; }

    public bool IsFeatured { get; set; } = false;
    
    public DateTime? FeaturedUntil { get; set; }

    public bool IsActive { get; set; } = true;

    public int EmployerId { get; set; }
    public Employer Employer { get; set; } = null!;

    public List<JobApplication> Applications { get; set; } = new();
    
    private JobPosting() { }

    public JobPosting(string title, string description, string location, int employerId, string employmentType = "FullTime")
    {
        if (string.IsNullOrWhiteSpace(title))
            throw new ArgumentException("Title Can't be empty", nameof(title));

        if (string.IsNullOrWhiteSpace(description))
            throw new ArgumentException("Description Can't be empty", nameof(description));

        if (employerId <= 0)
            throw new ArgumentException("JobPost Must be Attached to a valid employee", nameof(employerId));

        Title = title;
        Description = description;
        Location = location;
        EmployerId = employerId;
        EmploymentType = employmentType;
    }
}