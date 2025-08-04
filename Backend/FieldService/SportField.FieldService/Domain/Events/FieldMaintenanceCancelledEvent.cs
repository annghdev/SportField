using Common.Abstractions;

namespace SportField.FieldService.Domain.Events;

public record FieldMaintenanceCancelledEvent(
    string FieldId,
    string MaintenanceId,
    string Title,
    string? Reason
) : BaseDomainEvent; 