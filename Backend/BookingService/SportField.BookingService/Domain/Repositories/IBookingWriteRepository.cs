using Common.Abstractions;
using SportField.BookingService.Domain.Entities;

namespace SportField.BookingService.Domain.Repositories
{
    public interface IBookingWriteRepository : IWriteRepository<Booking, Guid>
    {
        // No custom write methods needed for now.
    }
}
