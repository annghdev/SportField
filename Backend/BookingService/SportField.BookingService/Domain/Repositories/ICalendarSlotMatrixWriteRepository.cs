using Common.Abstractions;
using SportField.BookingService.Domain.Entities;

namespace SportField.BookingService.Domain.Repositories
{
    public interface ICalendarSlotMatrixWriteRepository : IWriteRepository<CalendarSlotMatrix, Guid>
    {
        // No custom write methods needed for now.
    }
}
