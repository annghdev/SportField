using SportField.SharedKernel.DomainBase;

namespace SportField.FieldManagement.Domain.Events;

public record FieldActiveChangeEvent(Guid FieldId, bool IsActive) : BaseDomainEvent
{
}
