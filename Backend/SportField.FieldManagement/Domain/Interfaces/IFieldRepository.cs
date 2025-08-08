namespace SportField.FieldManagement.Domain.Interfaces;

public interface IFieldRepository : IRepository<Field, Guid>
{
    Task SetActiveAsync(Guid id, bool isActive);
    Task AddPricingAsync(FieldPricing pricing);
    Task ExtendEffectiveDateAsync(IEnumerable<Guid> pricingIds, DateTime effectiveTo);
    Task UpdatePricing(Dictionary<Guid, decimal> pricings); // <pricingId, price>
    Task DeletePricing(Guid pricingId);
}
