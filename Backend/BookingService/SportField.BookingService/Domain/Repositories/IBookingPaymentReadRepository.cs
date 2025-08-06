using Common.Abstractions;
using SportField.BookingService.Domain.Entities;

namespace SportField.BookingService.Domain.Repositories
{
    public interface IBookingPaymentReadRepository : IReadRepository<BookingPayment, Guid>
    {
        Task<IReadOnlyList<BookingPayment>> GetPaymentsByBookingIdAsync(Guid bookingId, CancellationToken cancellationToken = default);
        Task<IReadOnlyList<BookingPayment>> GetExpiredPendingPaymentsAsync(CancellationToken cancellationToken = default);
    }
}
