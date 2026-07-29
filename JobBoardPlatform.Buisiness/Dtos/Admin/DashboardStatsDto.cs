namespace JobBoardPlatform.Buisiness.Dtos.Admin;

public class DashboardStatsDto
{
    public int TotalJobSeekers { get; set; }
    public int TotalEmployers { get; set; }
    public int ActiveJobPostings { get; set; }
    public int InactiveJobPostings { get; set; }
    public Dictionary<string, int> ApplicationsByStatus { get; set; } = new();
    public int PendingEmployerApprovals { get; set; }
}