using Common.Abstractions;

namespace SportField.FieldService.Domain.Events;

/// <summary>
/// Event raised when the active status of an entire facility changes.
/// This event is used to synchronize the availability of all fields within the facility.
/// </summary>
/// <param name="FacilityId">The ID of the facility that changed status.</param>
/// <param name="IsActive">The new active status of the facility.</param>
public record FacilityStatusChangedEvent(
    Guid FacilityId,
    bool IsActive
) : BaseDomainEvent;
