using Common.Abstractions;

namespace SportField.FieldService.Domain.Events;

public record TimeSlotPriceUpdatedEvent(
    Guid FieldId,
    string TimeSlotId,
    decimal NewPrice,
    DayOfWeek? DayOfWeek
) : BaseDomainEvent; 