using Common.Abstractions;

namespace SportField.IdentityService.Domain.Events;

public record UserRoleChangedEvent(
    Guid UserId,
    Guid RoleId,
    string RoleName,
    string Action, // "Added", "Removed", "Activated", "Deactivated"
    string? ChangedBy = null,
    string? Reason = null
) : BaseDomainEvent;
