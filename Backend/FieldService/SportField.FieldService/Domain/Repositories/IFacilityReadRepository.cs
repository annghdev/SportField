using Common.Abstractions;
using SportField.FieldService.Domain.Entities;

namespace SportField.FieldService.Domain.Repositories
{
    public interface IFacilityReadRepository : IReadRepository<Facility, Guid>
    {
        // No custom read methods needed for now.
        // Can be extended with methods like:
        // Task<IEnumerable<Facility>> GetFacilitiesInCityAsync(string city);
    }
}
