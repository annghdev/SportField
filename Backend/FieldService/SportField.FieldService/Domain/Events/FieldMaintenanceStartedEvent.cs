using Common.Abstractions;

namespace SportField.FieldService.Domain.Events;

public record FieldMaintenanceStartedEvent(
    string FieldId,
    string MaintenanceId,
    string Title
) : BaseDomainEvent; 