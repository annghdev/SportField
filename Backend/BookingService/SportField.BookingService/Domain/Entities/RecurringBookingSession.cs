using Common.Abstractions;
using Common.Enums;

namespace SportField.BookingService.Domain.Entities;

/// <summary>
/// Từng buổi cụ thể trong Recurring booking - admin có thể đánh dấu từng buổi
/// </summary>
public class RecurringBookingSession : BaseEntity
{
    public Guid RecurringBookingDetailId { get; set; }
    public DateTime SessionDate { get; set; }
    public BookingStatus Status { get; set; } = BookingStatus.Confirmed;
    
    // Admin tracking
    public bool IsMarkedByAdmin { get; set; } = false; // Admin đánh dấu từng buổi
    public DateTime? MarkedDate { get; set; }
    public string? MarkedByAdminId { get; set; }
    public string? AdminNotes { get; set; }
    
    // Session specific
    public bool IsSkipped { get; set; } = false; // Bỏ qua buổi này
    public string? SkipReason { get; set; }
    public decimal SessionAmount { get; set; }
    public bool IsNoShow { get; set; } = false; // Khách không đến
    
    // Navigation properties
    public virtual RecurringBookingDetail RecurringBookingDetail { get; set; } = null!;

    public static RecurringBookingSession Create(Guid recurringBookingDetailId, DateTime sessionDate, decimal sessionAmount)
    {
        return new RecurringBookingSession
        {
            Id = Guid.CreateVersion7(),
            RecurringBookingDetailId = recurringBookingDetailId,
            SessionDate = sessionDate.Date,
            SessionAmount = sessionAmount,
        };
    }

    public void MarkByAdmin(string adminId, string? notes = null)
    {
        IsMarkedByAdmin = true;
        MarkedDate = DateTime.UtcNow;
        MarkedByAdminId = adminId;
        AdminNotes = notes;
    }

    public void Skip(string reason, string? adminId = null)
    {
        IsSkipped = true;
        SkipReason = reason;
        Status = BookingStatus.Cancelled;
        
        if (adminId != null)
        {
            MarkByAdmin(adminId, $"Skipped: {reason}");
        }
    }

    public void MarkAsNoShow(string? adminId = null)
    {
        IsNoShow = true;
        Status = BookingStatus.NoShow;
        
        if (adminId != null)
        {
            MarkByAdmin(adminId, "Marked as No Show");
        }
    }

    public void Complete(string? adminId = null)
    {
        Status = BookingStatus.Completed;
        
        if (adminId != null)
        {
            MarkByAdmin(adminId, "Session completed");
        }
    }

    public bool IsToday()
    {
        return SessionDate.Date == DateTime.Today;
    }

    public bool IsUpcoming()
    {
        return SessionDate.Date > DateTime.Today && Status == BookingStatus.Confirmed;
    }

    public bool IsPast()
    {
        return SessionDate.Date < DateTime.Today;
    }

    public string GetTimeSlotDisplay()
    {
        return RecurringBookingDetail?.GetTimeSlotDisplay() ?? "No time slots";
    }
}