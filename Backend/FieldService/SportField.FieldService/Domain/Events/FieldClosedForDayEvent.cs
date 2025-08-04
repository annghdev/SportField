using Common.Abstractions;

namespace SportField.FieldService.Domain.Events;

public record FieldClosedForDayEvent(
    Guid FieldId,
    DayOfWeek DayOfWeek,
    string? Reason
) : BaseDomainEvent; 