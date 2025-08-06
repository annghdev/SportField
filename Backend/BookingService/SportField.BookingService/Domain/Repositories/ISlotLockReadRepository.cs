using Common.Abstractions;
using SportField.BookingService.Domain.Entities;

namespace SportField.BookingService.Domain.Repositories
{
    public interface ISlotLockReadRepository : IReadRepository<SlotLock, Guid>
    {
        Task<IReadOnlyList<SlotLock>> GetExpiredLocksAsync(CancellationToken cancellationToken = default);
    }
}
