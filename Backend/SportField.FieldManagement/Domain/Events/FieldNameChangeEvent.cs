using SportField.SharedKernel.DomainBase;

namespace SportField.FieldManagement.Domain.Events;

public record FieldNameChangeEvent(Guid FieldId, string Name) : BaseDomainEvent
{
}
