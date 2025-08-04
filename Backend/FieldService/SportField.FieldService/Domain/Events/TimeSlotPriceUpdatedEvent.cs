using Common.Abstractions;

namespace SportField.FieldService.Domain.Events;

public record TimeSlotPriceUpdatedEvent(
    string CourtId,
    string TimeSlotId,
    decimal NewPrice,
    DayOfWeek? DayOfWeek
) : BaseDomainEvent; 