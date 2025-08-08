using SportField.SharedKernel.DomainBase;

namespace SportField.FieldManagement.Domain.Events;

public record FieldPricingDeleteEvent(
    Guid FieldId,
    Guid FieldPricingId) : BaseDomainEvent
{
}
