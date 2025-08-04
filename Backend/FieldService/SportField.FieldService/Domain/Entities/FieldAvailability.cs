using Common.Abstractions;

namespace SportField.FieldService.Domain.Entities;

public class FieldAvailability : BaseEntity<string>
{
    public required string FieldId { get; set; }
    public required string TimeSlotId { get; set; }
    public DateTime FromDate { get; set; }
    public DateTime? ToDate { get; set; } // null = vô thời hạn
    public bool IsBlocked { get; set; } = true; // true = khóa, false = mở
    public string? Reason { get; set; }

    // Navigation properties
    public virtual Field Field { get; set; } = null!;
    public virtual TimeSlot TimeSlot { get; set; } = null!;

    public static FieldAvailability CreateBlock(string FieldId, string timeSlotId, DateTime fromDate,
        DateTime? toDate = null, string? reason = null)
    {
        return new FieldAvailability
        {
            FieldId = FieldId,
            TimeSlotId = timeSlotId,
            FromDate = fromDate,
            ToDate = toDate,
            IsBlocked = true,
            Reason = reason
        };
    }

    public void UpdateBlockPeriod(DateTime fromDate, DateTime? toDate)
    {
        FromDate = fromDate;
        ToDate = toDate;
    }

    public void Unblock()
    {
        IsBlocked = false;
    }

    public bool IsBlockedOnDate(DateTime date)
    {
        if (!IsBlocked) return false;

        if (date < FromDate.Date) return false;

        if (ToDate.HasValue && date > ToDate.Value.Date) return false;

        return true;
    }
}
