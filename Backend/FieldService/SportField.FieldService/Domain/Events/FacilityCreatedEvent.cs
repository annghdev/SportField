using Common.Abstractions;

namespace SportField.FieldService.Domain.Events;

public record FacilityCreatedEvent(
    string FacilityId,
    string Name,
    string Address
) : BaseDomainEvent;
