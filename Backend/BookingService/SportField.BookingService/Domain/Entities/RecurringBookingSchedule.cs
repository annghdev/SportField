using Common.Abstractions;

namespace SportField.BookingService.Domain.Entities;

/// <summary>
/// Quản lý việc sinh schedule tự động cho Recurring booking
/// </summary>
public class RecurringBookingSchedule : BaseEntity
{
    public Guid RecurringBookingDetailId { get; set; }
    public DateTime ScheduleMonth { get; set; } // Tháng được sinh schedule
    public bool IsGenerated { get; set; } = false;
    public DateTime? GeneratedDate { get; set; }
    public int TotalSessions { get; set; } // Số buổi trong tháng
    public decimal MonthlyAmount { get; set; }
    public string? Notes { get; set; }

    // Navigation properties
    public virtual RecurringBookingDetail RecurringBookingDetail { get; set; } = null!;

    public static RecurringBookingSchedule Create(Guid recurringBookingDetailId, DateTime scheduleMonth)
    {
        return new RecurringBookingSchedule
        {
            Id = Guid.CreateVersion7(),
            RecurringBookingDetailId = recurringBookingDetailId,
            ScheduleMonth = new DateTime(scheduleMonth.Year, scheduleMonth.Month, 1), // First day of month
        };
    }

    public void MarkAsGenerated(int totalSessions, decimal monthlyAmount)
    {
        IsGenerated = true;
        GeneratedDate = DateTime.UtcNow;
        TotalSessions = totalSessions;
        MonthlyAmount = monthlyAmount;
    }

    public bool IsCurrentMonth()
    {
        var now = DateTime.Now;
        return ScheduleMonth.Year == now.Year && ScheduleMonth.Month == now.Month;
    }

    public bool IsFutureMonth()
    {
        var now = DateTime.Now;
        return ScheduleMonth > new DateTime(now.Year, now.Month, 1);
    }
}