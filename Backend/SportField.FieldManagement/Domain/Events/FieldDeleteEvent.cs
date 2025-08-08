using SportField.SharedKernel.DomainBase;

namespace SportField.FieldManagement.Domain.Events;

public record FieldDeleteEvent(Guid FieldId) : BaseDomainEvent
{
}
