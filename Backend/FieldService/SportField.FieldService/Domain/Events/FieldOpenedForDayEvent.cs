using Common.Abstractions;

namespace SportField.FieldService.Domain.Events;

public record FieldOpenedForDayEvent(
    string FieldId,
    DayOfWeek DayOfWeek
) : BaseDomainEvent; 