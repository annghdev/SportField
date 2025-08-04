using Common.Abstractions;

namespace SportField.FieldService.Domain.Events;

public record FieldClosedForDayEvent(
    string FieldId,
    DayOfWeek DayOfWeek,
    string? Reason
) : BaseDomainEvent; 