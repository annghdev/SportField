using Common.Abstractions;

namespace SportField.FieldService.Domain.Events;

public record FieldMaintenanceRescheduledEvent(
    Guid FieldId,
    Guid MaintenanceId,
    DateTime OldStartTime,
    DateTime OldEndTime,
    DateTime NewStartTime,
    DateTime NewEndTime
) : BaseDomainEvent; 