namespace JobBoardPlatform.Buisiness.Dtos.JobPosting;

public class JobPostingSearchQueryDto
{
    public string? Search { get; set; }
    public string? EmploymentType { get; set; }
    public string? City { get; set; }
    public string? Category { get; set; }
    public decimal? MinSalary { get; set; }
    public decimal? MaxSalary { get; set; }
    public string? Skill { get; set; }

    private const int MaxPageSize = 50;
    public int Page { get; set; } = 1;

    private int _pageSize = 10;
    public int PageSize
    {
        get => _pageSize;
        set => _pageSize = value > MaxPageSize ? MaxPageSize : value;
    }
}