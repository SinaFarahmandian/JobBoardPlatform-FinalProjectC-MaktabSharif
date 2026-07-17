namespace JobBoardPlatform.Buisiness.Dtos.JobApplication;

public class EmployerSummaryDto
{
    public int Id { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string CompanyName { get; set; } = string.Empty;
}