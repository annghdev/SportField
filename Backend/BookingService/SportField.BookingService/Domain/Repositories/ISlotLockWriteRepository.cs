using Common.Abstractions;
using SportField.BookingService.Domain.Entities;

namespace SportField.BookingService.Domain.Repositories
{
    public interface ISlotLockWriteRepository : IWriteRepository<SlotLock, Guid>
    {
        // No custom write methods needed for now.
    }
}
