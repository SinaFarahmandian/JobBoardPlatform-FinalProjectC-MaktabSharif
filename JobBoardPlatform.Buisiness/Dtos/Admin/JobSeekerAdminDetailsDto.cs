namespace JobBoardPlatform.Buisiness.Dtos.Admin;

public class JobSeekerAdminDetailsDto : JobSeekerAdminDto
{
    public string? Skills { get; set; }
    public int YearsOfExperience { get; set; }
    public string? DesiredJobTitle { get; set; }
    public bool HasResume { get; set; }
    public int ApplicationsCount { get; set; }
}