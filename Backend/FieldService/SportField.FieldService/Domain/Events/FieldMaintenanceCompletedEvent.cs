using Common.Abstractions;

namespace SportField.FieldService.Domain.Events;

public record FieldMaintenanceCompletedEvent(
    Guid FieldId,
    Guid MaintenanceId,
    string Title,
    decimal? ActualCost
) : BaseDomainEvent; 