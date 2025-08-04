using Common.Abstractions;

namespace SportField.FieldService.Domain.Events;

public record FieldOperatingHoursUpdatedEvent(
    Guid FieldId,
    DayOfWeek DayOfWeek,
    TimeOnly OpenTime,
    TimeOnly CloseTime,
    bool IsActive
) : BaseDomainEvent; 