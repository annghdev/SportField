using Common.Abstractions;
using Common.Enums;

namespace SportField.BookingService.Domain.Events;

public record BookingCancelledEvent(
    Guid BookingId,
    BookingType BookingType,
    Guid FacilityId,
    List<(Guid FieldId, string TimeSlotId)> CancelledSlots,
    Guid? UserId,
    DateTime CancelledDate,
    string CancellationReason
) : BaseDomainEvent;