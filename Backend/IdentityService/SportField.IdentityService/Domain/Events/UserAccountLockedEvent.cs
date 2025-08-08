using Common.Abstractions;

namespace SportField.IdentityService.Domain.Events;

public record UserAccountLockedEvent(
    Guid UserId,
    string Email,
    string PhoneNumber,
    DateTime LockoutEndDate,
    string Reason,
    int FailedAttempts
) : BaseDomainEvent;
