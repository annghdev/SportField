namespace SportField.FieldManagement.Domain.Exceptions
{
    public class PricingEffectiveDateException(Guid fieldPricingId) : Exception($"FieldPricing {fieldPricingId} has no effective date")
    {
    }
}
