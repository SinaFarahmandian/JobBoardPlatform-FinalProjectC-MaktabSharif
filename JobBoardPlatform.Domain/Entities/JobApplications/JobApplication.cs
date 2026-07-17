using System.ComponentModel.DataAnnotations;
using JobBoardPlatform.Domain.Entities.JobPostings;
using JobBoardPlatform.Domain.Entities.JobSeekers;
using JobBoardPlatform.Domain.Enums;

namespace JobBoardPlatform.Domain.Entities.JobApplications;

public class JobApplication : BaseEntity
{
    public int JobPostingId { get; set; }
    public JobPosting JobPosting { get; set; } = null!;

    public int JobSeekerId { get; set; }
    public JobSeeker JobSeeker { get; set; } = null!;

    public ApplicationStatus Status { get; set; } = ApplicationStatus.Pending;

    [StringLength(3000)]
    public string? CoverLetter { get; set; }

    [StringLength(1000)]
    public string? EmployerNotes { get; set; }

    private JobApplication() { }

    public JobApplication(int jobPostingId, int jobSeekerId, string? coverLetter = null)
    {
        if (jobPostingId <= 0)
            throw new ArgumentException("JobApplication Must be Attached to a valid JobPost", nameof(jobPostingId));

        if (jobSeekerId <= 0)
            throw new ArgumentException("JobApplication Must be Attached to a valid JobSeeker", nameof(jobSeekerId));

        JobPostingId = jobPostingId;
        JobSeekerId = jobSeekerId;
        CoverLetter = coverLetter;
        Status = ApplicationStatus.Pending;
    }
}