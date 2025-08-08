using SportField.SharedKernel.DomainBase;

namespace SportField.FieldManagement.Domain.Events;

public record FieldPricingAddEvent(
    Guid FieldId,
    Guid FieldPricingId,
    string timeSlotId,
    decimal Price,
    DateTime? EffectiveFrom,
    DateTime? EffectiveTo) : BaseDomainEvent
{
}
