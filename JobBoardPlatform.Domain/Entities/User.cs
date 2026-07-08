using System.ComponentModel.DataAnnotations;
using JobBoardPlatform.Domain.Enums;

namespace JobBoardPlatform.Domain.Entities;

public class User : BaseEntity
{
    [Required, StringLength(100, MinimumLength = 2)]
    public string FullName { get; set; } = string.Empty;

    [Required, EmailAddress, StringLength(150)]
    public string Email { get; set; } = string.Empty;

    [Required]
    public string PasswordHash { get; set; } = string.Empty;

    [Phone, StringLength(20)]
    public string? PhoneNumber { get; set; }

    public UserRole Role { get; protected set; }

    public bool IsActive { get; set; } = true;

    protected User() { }

    protected User(string fullName, string email, string passwordHash)
    {
        if (string.IsNullOrWhiteSpace(fullName))
            throw new ArgumentException("Fullname Can't Be Empty", nameof(fullName));

        if (string.IsNullOrWhiteSpace(email))
            throw new ArgumentException("Email Can't Be Empty", nameof(email));

        if (string.IsNullOrWhiteSpace(passwordHash))
            throw new ArgumentException("Password Can't Be Empty", nameof(passwordHash));

        FullName = fullName;
        Email = email;
        PasswordHash = passwordHash;
        CreatedAt = DateTime.UtcNow;
        IsActive = true;
    }
    
    
}