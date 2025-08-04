using Common.Abstractions;
using Common.Enums;

namespace SportField.FieldService.Domain.Events;

public record FieldMaintenanceScheduledEvent(
    Guid FieldId,
    Guid MaintenanceId,
    DateTime StartTime,
    DateTime EndTime,
    MaintenanceType Type
) : BaseDomainEvent;
