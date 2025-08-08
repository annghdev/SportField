namespace SportField.FieldManagement.Domain.Interfaces;

public interface IFacilityRepository : IRepository<Facility, Guid>
{
    Task SetActive(Guid id, bool isActive);
}
