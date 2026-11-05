using Microsoft.AspNetCore.Mvc;
using ONE_Classics_Nexus.API.Contracts.Users;
using ONE_Classics_Nexus.Application.Abstractions.Repositories;
using ONE_Classics_Nexus.Domain.Entities;

namespace ONE_Classics_Nexus.API.Controllers;

[ApiController]
[Route("api/users")]
public sealed class UsersController : ControllerBase
{
    private readonly IUserRepository _repository;

    public UsersController(IUserRepository repository) => _repository = repository;

    [HttpPost]
    public async Task<IActionResult> Create(CreateUserRequest request, CancellationToken cancellationToken)
    {
        var now = DateTime.UtcNow;
        var user = new User
        {
            UserId = Guid.NewGuid(), Email = request.Email.Trim(), Username = request.Username.Trim(),
            DisplayName = request.DisplayName.Trim(), FirstName = Normalize(request.FirstName),
            LastName = Normalize(request.LastName), AuthProvider = Normalize(request.AuthProvider),
            ExternalSubject = Normalize(request.ExternalSubject), IsActive = request.IsActive,
            CreatedAtUtc = now, UpdatedAtUtc = now
        };

        var result = await _repository.CreateAsync(user, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { userId = result.UserId }, ToResponse(result));
    }

    [HttpGet("{userId:guid}")]
    public async Task<IActionResult> GetById(Guid userId, CancellationToken cancellationToken)
    {
        var user = await _repository.GetByIdAsync(userId, cancellationToken);
        return user is null ? NotFound() : Ok(ToResponse(user));
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<UserResponse>>> List(CancellationToken cancellationToken)
    {
        var users = await _repository.ListAsync(cancellationToken);
        return Ok(users.Select(ToResponse).ToArray());
    }

    [HttpPut("{userId:guid}")]
    public async Task<IActionResult> Update(Guid userId, UpdateUserRequest request, CancellationToken cancellationToken)
    {
        var existing = await _repository.GetByIdAsync(userId, cancellationToken);
        if (existing is null) return NotFound();

        existing.Email = request.Email.Trim(); existing.Username = request.Username.Trim();
        existing.DisplayName = request.DisplayName.Trim(); existing.FirstName = Normalize(request.FirstName);
        existing.LastName = Normalize(request.LastName); existing.AuthProvider = Normalize(request.AuthProvider);
        existing.ExternalSubject = Normalize(request.ExternalSubject); existing.IsActive = request.IsActive;
        existing.UpdatedAtUtc = DateTime.UtcNow;

        var result = await _repository.UpdateAsync(existing, cancellationToken);
        return result is null ? NotFound() : Ok(ToResponse(result));
    }

    [HttpDelete("{userId:guid}")]
    public async Task<IActionResult> Delete(Guid userId, CancellationToken cancellationToken)
    {
        var deleted = await _repository.DeleteAsync(userId, cancellationToken);
        return deleted ? NoContent() : NotFound();
    }

    private static string? Normalize(string? value) => string.IsNullOrWhiteSpace(value) ? null : value.Trim();

    private static UserResponse ToResponse(User user) => new()
    {
        UserId = user.UserId, Email = user.Email, Username = user.Username, DisplayName = user.DisplayName,
        FirstName = user.FirstName, LastName = user.LastName, AuthProvider = user.AuthProvider,
        ExternalSubject = user.ExternalSubject, IsActive = user.IsActive,
        CreatedAtUtc = user.CreatedAtUtc, UpdatedAtUtc = user.UpdatedAtUtc
    };
}
