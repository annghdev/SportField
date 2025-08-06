using Common.Abstractions;
using SportField.FieldService.Domain.Entities;

namespace SportField.FieldService.Domain.Repositories
{
    public interface IFieldReadRepository : IReadRepository<Field, Guid>
    {

        Task<Field?> GetFieldWithDetailsByIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task<IReadOnlyList<Field>> GetActiveFieldsByFacilityIdAsync(Guid facilityId, CancellationToken cancellationToken = default);
    }
}
