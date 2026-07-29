namespace JobBoardPlatform.Buisiness.Dtos.Admin;

public class EmployerAdminDetailsDto : EmployerAdminDto
{
    public string? CompanyWebsite { get; set; }
    public string? CompanyDescription { get; set; }
    public string? Industry { get; set; }
    public int JobPostingsCount { get; set; }
}