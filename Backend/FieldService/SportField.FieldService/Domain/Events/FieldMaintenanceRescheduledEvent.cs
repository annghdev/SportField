using Common.Abstractions;

namespace SportField.FieldService.Domain.Events;

public record FieldMaintenanceRescheduledEvent(
    string FieldId,
    string MaintenanceId,
    DateTime OldStartTime,
    DateTime OldEndTime,
    DateTime NewStartTime,
    DateTime NewEndTime
) : BaseDomainEvent; 