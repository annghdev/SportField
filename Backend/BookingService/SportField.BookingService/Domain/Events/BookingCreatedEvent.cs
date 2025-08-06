using Common.Abstractions;
using Common.Enums;

namespace SportField.BookingService.Domain.Events;

public record BookingCreatedEvent(
    Guid BookingId,
    BookingType BookingType,
    BookingOrigin Origin,
    Guid FacilityId,
    List<Guid> FieldIds,
    Guid? UserId,
    DateTime BookingDate,
    decimal TotalAmount
) : BaseDomainEvent;