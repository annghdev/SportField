using Common.Abstractions;
using SportField.FieldService.Domain.Entities;

namespace SportField.FieldService.Domain.Repositories
{
    public interface IFacilityWriteRepository : IWriteRepository<Facility, Guid>
    {
        // No custom write methods needed for now.
    }
}
