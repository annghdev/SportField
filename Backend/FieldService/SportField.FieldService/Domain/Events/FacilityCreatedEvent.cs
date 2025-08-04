using Common.Abstractions;

namespace SportField.FieldService.Domain.Events;

public record FacilityCreatedEvent(
    Guid FacilityId,
    string Name,
    string Address
) : BaseDomainEvent;
