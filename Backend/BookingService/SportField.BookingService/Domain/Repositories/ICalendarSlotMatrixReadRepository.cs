using Common.Abstractions;
using SportField.BookingService.Domain.Entities;

namespace SportField.BookingService.Domain.Repositories
{
    public interface ICalendarSlotMatrixReadRepository : IReadRepository<CalendarSlotMatrix, Guid>
    {
        Task<IReadOnlyList<CalendarSlotMatrix>> GetSlotsByDateRangeAsync(Guid facilityId, DateTime startDate, DateTime endDate, CancellationToken cancellationToken = default);
        Task<IReadOnlyList<CalendarSlotMatrix>> GetSlotsByCompositeKeysAsync(IEnumerable<(Guid FieldId, string TimeSlotId, DateTime Date)> slotsToFind, CancellationToken cancellationToken = default);
    }
}
