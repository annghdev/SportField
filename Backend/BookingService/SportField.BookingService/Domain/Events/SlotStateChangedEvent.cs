using Common.Abstractions;
using Common.Enums;

namespace SportField.BookingService.Domain.Events;

public record SlotStateChangedEvent(
    Guid FieldId,
    string TimeSlotId,
    DateTime Date,
    SlotState FromState,
    SlotState ToState,
    Guid? BookingId = null,
    string? SessionId = null
) : BaseDomainEvent;