using Common.Abstractions;
using Common.Enums;

namespace SportField.BookingService.Domain.Events;

public record BookingConfirmedEvent(
    Guid BookingId,
    BookingType BookingType,
    Guid FacilityId,
    List<(Guid FieldId, string TimeSlotId)> BookedSlots,
    Guid? UserId,
    DateTime ConfirmedDate
) : BaseDomainEvent;