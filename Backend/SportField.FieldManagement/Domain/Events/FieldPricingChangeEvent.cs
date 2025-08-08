using SportField.SharedKernel.DomainBase;

namespace SportField.FieldManagement.Domain.Events;

public record FieldPricingChangeEvent(
    Guid FieldId,
    Guid FieldPricingId,
    decimal Price) : BaseDomainEvent
{
}
