using Common.Abstractions;

namespace SportField.FieldService.Domain.Events;

public record FieldMaintenanceCompletedEvent(
    string FieldId,
    string MaintenanceId,
    string Title,
    decimal? ActualCost
) : BaseDomainEvent; 