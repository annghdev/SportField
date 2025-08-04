using Common.Abstractions;

namespace SportField.FieldService.Domain.Events;

public record FieldMaintenanceStartedEvent(
    Guid FieldId,
    Guid MaintenanceId,
    string Title
) : BaseDomainEvent; 