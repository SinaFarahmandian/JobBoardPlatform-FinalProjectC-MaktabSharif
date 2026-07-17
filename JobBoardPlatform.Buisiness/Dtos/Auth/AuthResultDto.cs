namespace JobBoardPlatform.Buisiness.Dtos.Auth;

// AuthResultDto.cs
public class AuthResultDto
{
    public bool Succeeded { get; set; }
    public string? Token { get; set; }
    public string? FullName { get; set; }
    public string? Email { get; set; }
    public string? Role { get; set; }
    public string? Message { get; set; }
    public IEnumerable<string> Errors { get; set; } = Enumerable.Empty<string>();

    public static AuthResultDto Success(string message) => new() { Succeeded = true, Message = message };
    public static AuthResultDto Failure(IEnumerable<string> errors) => new() { Succeeded = false, Errors = errors };
    public static AuthResultDto SuccessWithToken(string token, string fullName, string email, string role)
        => new() { Succeeded = true, Token = token, FullName = fullName, Email = email, Role = role };
}