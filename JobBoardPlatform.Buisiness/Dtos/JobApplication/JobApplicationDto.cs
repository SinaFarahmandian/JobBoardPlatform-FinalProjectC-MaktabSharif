using System.ComponentModel.DataAnnotations;
using JobBoardPlatform.Domain.Enums;

namespace JobBoardPlatform.Buisiness.Dtos.JobApplication;

public class JobApplicationDto
{
    public int Id { get; set; }
    public int JobPostingId { get; set; }
    public string JobPostingTitle { get; set; } = string.Empty;
    public int JobSeekerId { get; set; }
    public string JobSeekerName { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public string? CoverLetter { get; set; }
    public DateTime AppliedAt { get; set; }
}



