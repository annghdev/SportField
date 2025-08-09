namespace SportField.FieldManagement.Domain.Events;

public record FieldMaintenanceDeleteEvent(Guid FieldId, Guid MaintenanceId) : BaseDomainEvent;