namespace JobBoardPlatform.Buisiness.Dtos.JobApplication;

public class JobSeekerApplicationDto
{
    public int Id { get; set; }
    public int JobPostingId { get; set; }
    public string JobPostingTitle { get; set; } = string.Empty;
    public string CompanyName { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public DateTime AppliedAt { get; set; }
}