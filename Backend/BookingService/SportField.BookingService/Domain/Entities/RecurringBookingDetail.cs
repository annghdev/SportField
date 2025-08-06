using Common.Abstractions;
using Common.Enums;

namespace SportField.BookingService.Domain.Entities;

/// <summary>
/// Chi tiết cho Recurring booking - sử dụng TimeSlots liên tục, lặp lại theo chu kỳ
/// </summary>
public class RecurringBookingDetail : BaseEntity
{
    public Guid BookingId { get; set; }
    
    // Fields và TimeSlots (phải chỉ định 1 sân và các slot liên tục)
    public Guid FieldId { get; set; } // Recurring booking chỉ cho 1 sân
    public string TimeSlotIds { get; set; } = string.Empty; // JSON: ["slot1","slot2","slot3"] - phải liên tục
    
    // Chu kỳ lặp lại
    public DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; } // Có thể bỏ trống
    public string DaysOfWeek { get; set; } = string.Empty; // JSON: [1,3,5] = thứ 2,4,6
    
    // Trạng thái và quản lý
    public RecurringBookingStatus RecurringStatus { get; set; } = RecurringBookingStatus.Active;
    public DateTime? SuspendedDate { get; set; }
    public DateTime? ResumedDate { get; set; }
    public string? SuspensionReason { get; set; }
    
    // Pricing
    public decimal BasePrice { get; set; }
    public decimal DiscountPercentage { get; set; } = 0; // Giảm giá cho đặt định kỳ
    public decimal MonthlyAmount { get; set; }
    
    // Navigation properties
    public virtual Booking Booking { get; set; } = null!;
    public virtual ICollection<RecurringBookingSchedule> Schedules { get; set; } = [];
    public virtual ICollection<RecurringBookingSession> Sessions { get; set; } = [];

    public static RecurringBookingDetail Create(Guid bookingId, Guid fieldId, List<string> timeSlotIds,
        DateTime startDate, DateTime? endDate, List<DayOfWeek> daysOfWeek, decimal basePrice, decimal discountPercentage = 0)
    {
        // Validate minimum 1 month
        var actualEndDate = endDate ?? startDate.AddMonths(1);
        if (actualEndDate < startDate.AddMonths(1))
            throw new ArgumentException("Recurring booking must be at least 1 month");

        if (timeSlotIds.Count == 0)
            throw new ArgumentException("At least one time slot must be specified");

        return new RecurringBookingDetail
        {
            Id = Guid.CreateVersion7(),
            BookingId = bookingId,
            FieldId = fieldId,
            TimeSlotIds = System.Text.Json.JsonSerializer.Serialize(timeSlotIds),
            StartDate = startDate.Date,
            EndDate = actualEndDate.Date,
            DaysOfWeek = System.Text.Json.JsonSerializer.Serialize(daysOfWeek.Select(d => (int)d)),
            BasePrice = basePrice,
            DiscountPercentage = discountPercentage,
            MonthlyAmount = CalculateMonthlyAmount(basePrice, discountPercentage, daysOfWeek.Count),
        };
    }

    private static decimal CalculateMonthlyAmount(decimal basePrice, decimal discountPercentage, int daysPerWeek)
    {
        var weeklyAmount = basePrice * daysPerWeek;
        var monthlyAmount = weeklyAmount * 4.33m; // Average weeks per month
        return monthlyAmount * (1 - discountPercentage / 100);
    }

    public void Suspend(string reason)
    {
        RecurringStatus = RecurringBookingStatus.Suspended;
        SuspendedDate = DateTime.UtcNow;
        SuspensionReason = reason;
    }

    public void Resume()
    {
        RecurringStatus = RecurringBookingStatus.Active;
        ResumedDate = DateTime.UtcNow;
    }

    public void ExtendEndDate(DateTime newEndDate)
    {
        EndDate = newEndDate;
        RecurringStatus = RecurringBookingStatus.Extended;
    }

    public List<DayOfWeek> GetDaysOfWeek()
    {
        if (string.IsNullOrEmpty(DaysOfWeek))
            return [];

        var dayNumbers = System.Text.Json.JsonSerializer.Deserialize<List<int>>(DaysOfWeek);
        return dayNumbers?.Select(d => (DayOfWeek)d).ToList() ?? [];
    }

    public List<string> GetTimeSlotIds()
    {
        if (string.IsNullOrEmpty(TimeSlotIds))
            return [];

        return System.Text.Json.JsonSerializer.Deserialize<List<string>>(TimeSlotIds) ?? [];
    }

    public void UpdateTimeSlots(List<string> timeSlotIds)
    {
        if (timeSlotIds.Count == 0)
            throw new ArgumentException("At least one time slot must be specified");
            
        TimeSlotIds = System.Text.Json.JsonSerializer.Serialize(timeSlotIds);
    }

    public bool IsActiveOnDate(DateTime date)
    {
        if (RecurringStatus != RecurringBookingStatus.Active) return false;
        if (date.Date < StartDate || (EndDate.HasValue && date.Date > EndDate.Value)) return false;
        
        var daysOfWeek = GetDaysOfWeek();
        return daysOfWeek.Contains(date.DayOfWeek);
    }

    public string GetTimeSlotDisplay()
    {
        var slots = GetTimeSlotIds();
        return slots.Count > 0 ? string.Join(", ", slots) : "No time slots";
    }
}