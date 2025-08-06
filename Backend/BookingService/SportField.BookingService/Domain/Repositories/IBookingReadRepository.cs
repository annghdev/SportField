using Common.Abstractions;
using SportField.BookingService.Domain.Entities;

namespace SportField.BookingService.Domain.Repositories
{
    public interface IBookingReadRepository : IReadRepository<Booking, Guid>
    {
        Task<Booking?> GetBookingWithDetailsByIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task<IReadOnlyList<Booking>> GetBookingsByUserIdAsync(Guid userId, CancellationToken cancellationToken = default);
    }
}
