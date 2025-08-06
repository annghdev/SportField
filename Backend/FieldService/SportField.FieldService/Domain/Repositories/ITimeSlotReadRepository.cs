using Common.Abstractions;
using SportField.FieldService.Domain.Entities;

namespace SportField.FieldService.Domain.Repositories
{
    public interface ITimeSlotReadRepository : IReadRepository<TimeSlot, string>
    {
        Task<IReadOnlyList<TimeSlot>> GetTimeSlotsByIdsAsync(IEnumerable<string> ids, CancellationToken cancellationToken = default);
    }
}
