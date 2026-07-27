namespace JobBoardPlatform.Buisiness.Dtos.JobSeekerProfile;

public class JobSeekerProfileDto
{
    public int Id { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string? PhoneNumber { get; set; }
    public bool HasResume { get; set; }
    public string? ResumeFileName { get; set; }
    public string? Skills { get; set; }
    public int YearsOfExperience { get; set; }
    public string? DesiredJobTitle { get; set; }
}