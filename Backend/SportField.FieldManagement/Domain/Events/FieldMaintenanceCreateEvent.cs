namespace SportField.FieldManagement.Domain.Events;

public record FieldMaintenanceCreateEvent(
    Guid FieldId,
    Guid MaintenanceId,
    string TimeFrameId,
    DateTime Date,
    DateTime? RecurringTo,
    RecurringType RecurringType) : BaseDomainEvent;