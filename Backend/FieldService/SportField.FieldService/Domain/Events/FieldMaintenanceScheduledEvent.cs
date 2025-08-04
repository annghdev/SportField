using Common.Abstractions;
using Common.Enums;

namespace SportField.FieldService.Domain.Events;

public record FieldMaintenanceScheduledEvent(
    string FieldId,
    string MaintenanceId,
    DateTime StartTime,
    DateTime EndTime,
    MaintenanceType Type
) : BaseDomainEvent;
