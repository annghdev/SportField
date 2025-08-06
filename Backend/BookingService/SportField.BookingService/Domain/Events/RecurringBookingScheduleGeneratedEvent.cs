using Common.Abstractions;

namespace SportField.BookingService.Domain.Events;

public record RecurringBookingScheduleGeneratedEvent(
    Guid BookingId,
    Guid RecurringDetailId,
    DateTime ScheduleMonth,
    int TotalSessions,
    decimal MonthlyAmount,
    List<DateTime> SessionDates
) : BaseDomainEvent;