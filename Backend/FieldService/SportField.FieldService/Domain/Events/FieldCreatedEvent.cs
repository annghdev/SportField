using Common.Abstractions;
using Common.Enums;

namespace SportField.FieldService.Domain.Events;

public record FieldCreatedEvent(
    Guid FieldId,
    string Name,
    Guid FacilityId,
    FieldType Type
) : BaseDomainEvent; 