using Common.Abstractions;

namespace SportField.IdentityService.Domain.Events;

public record UserRegisteredEvent(
    Guid UserId,
    string Email,
    string PhoneNumber,
    string FullName,
    string RegistrationMethod // "Email", "Phone", "Google", "Facebook"
) : BaseDomainEvent;
