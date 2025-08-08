using Common.Abstractions;

namespace SportField.IdentityService.Domain.Events;

public record UserLoggedInEvent(
    Guid UserId,
    string LoginMethod, // "Password", "Google", "Facebook"
    string? IpAddress = null,
    string? UserAgent = null
) : BaseDomainEvent
{
    public DateTime LoginTime { get; init; } = DateTime.UtcNow;
}
