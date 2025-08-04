using Common.Abstractions;

namespace SportField.FieldService.Domain.Events;

public record FieldOperatingHoursUpdatedEvent(
    string FieldId,
    DayOfWeek DayOfWeek,
    TimeOnly OpenTime,
    TimeOnly CloseTime,
    bool IsActive
) : BaseDomainEvent; 