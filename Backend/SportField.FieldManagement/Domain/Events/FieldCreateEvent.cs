using SportField.SharedKernel.DomainBase;
using SportField.SharedKernel.Enums;

namespace SportField.FieldManagement.Domain.Events;

public record FieldCreateEvent(
    Guid FacilityId,
    Guid FieldId,
    string Name,
    FieldType FieldType,
    bool IsActive) : BaseDomainEvent
{
}
