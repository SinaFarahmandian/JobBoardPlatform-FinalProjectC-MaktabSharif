using System.ComponentModel.DataAnnotations;

namespace JobBoardPlatform.Buisiness.Dtos.Company;

public class CompanyDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Website { get; set; }
    public string? Description { get; set; }
    public string? Industry { get; set; }
}

