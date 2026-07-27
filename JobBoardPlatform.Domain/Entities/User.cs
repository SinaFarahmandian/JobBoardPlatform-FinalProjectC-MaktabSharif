using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace JobBoardPlatform.Domain.Entities;

public abstract class User : IdentityUser<int>
{
    [Required, StringLength(100, MinimumLength = 2)]
    public string FullName { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
    public bool IsDeleted { get; set; } = false;

    public bool IsActive { get; set; } = true;

    public bool IsApproved { get; set; } = true;

    protected User() { }

    protected User(string fullName, string email)
    {
        FullName = fullName;
        Email = email;
        UserName = email;
        CreatedAt = DateTime.UtcNow;
    }
}