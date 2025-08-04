using Common.Abstractions;
using Common.Enums;

namespace SportField.FieldService.Domain.Events;

public record FieldCreatedEvent(
    string FieldId,
    string Name,
    string FacilityId,
    FieldType Type
) : BaseDomainEvent; 