namespace SportField.FieldManagement.Domain.Events;

public record FieldMaintenanceActiveChangeEvent(
    Guid FieldId,
    Guid MaintenanceId,
    bool IsActive) : BaseDomainEvent;