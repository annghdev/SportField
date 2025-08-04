using Common.Abstractions;

namespace SportField.FieldService.Domain.Events;

public record FieldOpenedForDayEvent(
    Guid FieldId,
    DayOfWeek DayOfWeek
) : BaseDomainEvent; 