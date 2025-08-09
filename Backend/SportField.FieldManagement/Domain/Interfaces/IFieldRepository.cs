namespace SportField.FieldManagement.Domain.Interfaces;

public interface IFieldRepository : IRepository<Field, Guid>
{
    Task SetActiveAsync(Guid id, bool isActive);

    Task AddPricingAsync(FieldPricing pricing);
    Task UpdatePricing(Dictionary<Guid, decimal> pricings); // <pricingId, price>
    Task ExtendPricingEffectiveDateAsync(IEnumerable<Guid> pricingIds, DateTime effectiveTo);
    Task DeletePricing(Guid pricingId);

    Task AddMaintenance(FieldMaintenance maintenance);
    Task SetMaintenanceActive(Guid maintenanceId, bool isActive);
    Task DeleteMaintenance(Guid maintenanceId);
}
