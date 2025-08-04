using Common.Abstractions;

namespace SportField.FieldService.Domain.Events;

public record FieldMaintenanceCancelledEvent(
    Guid FieldId,
    Guid MaintenanceId,
    string Title,
    string? Reason
) : BaseDomainEvent; 