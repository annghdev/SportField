using Common.Abstractions;

namespace SportField.BookingService.Domain.Events;

/// <summary>
/// Event raised when a conflict is detected between a confirmed booking and an administrative action
/// (e.g., scheduling maintenance, blocking slots for an event).
/// This event triggers notifications to administrators to manually resolve the conflict.
/// </summary>
public record BookingConflictDetectedEvent(
    Guid BookingId,
    string ConflictReason,
    Guid? AdminToNotifyId
) : BaseDomainEvent;
