using System.ComponentModel.DataAnnotations;

namespace ONE_Classics_Nexus.API.Contracts.Users;

public class CreateUserRequest
{
    [Required, EmailAddress, MaxLength(255)]
    public string Email { get; set; } = string.Empty;

    [Required, MaxLength(100)]
    public string Username { get; set; } = string.Empty;

    [Required, MaxLength(150)]
    public string DisplayName { get; set; } = string.Empty;

    [MaxLength(100)] public string? FirstName { get; set; }
    [MaxLength(100)] public string? LastName { get; set; }
    [MaxLength(50)] public string? AuthProvider { get; set; }
    [MaxLength(255)] public string? ExternalSubject { get; set; }
    public bool IsActive { get; set; } = true;
}

public sealed class UpdateUserRequest : CreateUserRequest
{
}

public sealed class UserResponse
{
    public Guid UserId { get; init; }
    public string Email { get; init; } = string.Empty;
    public string Username { get; init; } = string.Empty;
    public string DisplayName { get; init; } = string.Empty;
    public string? FirstName { get; init; }
    public string? LastName { get; init; }
    public string? AuthProvider { get; init; }
    public string? ExternalSubject { get; init; }
    public bool IsActive { get; init; }
    public DateTime CreatedAtUtc { get; init; }
    public DateTime UpdatedAtUtc { get; init; }
}
