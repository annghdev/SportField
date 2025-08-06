using Common.Abstractions;
using Common.Enums;

namespace SportField.BookingService.Domain.Entities;

/// <summary>
/// Lịch sử thay đổi trạng thái của booking
/// </summary>
public class BookingStatusHistory : BaseEntity
{
    public Guid BookingId { get; set; }
    public BookingStatus FromStatus { get; set; }
    public BookingStatus ToStatus { get; set; }
    public string? ChangeReason { get; set; }
    public Guid? ChangedByUserId { get; set; } // User hoặc Admin thực hiện thay đổi
    public DateTime ChangedDate { get; set; }

    // Navigation properties
    public virtual Booking Booking { get; set; } = null!;

    public static BookingStatusHistory Create(Guid bookingId, BookingStatus toStatus, 
        string? reason = null, Guid? changedByUserId = null, BookingStatus fromStatus = BookingStatus.Pending)
    {
        return new BookingStatusHistory
        {
            Id = Guid.CreateVersion7(),
            BookingId = bookingId,
            FromStatus = fromStatus,
            ToStatus = toStatus,
            ChangeReason = reason,
            ChangedByUserId = changedByUserId,
            ChangedDate = DateTime.UtcNow,
        };
    }

    public string GetStatusChangeDescription()
    {
        return $"{FromStatus} → {ToStatus}";
    }

    public bool IsStatusUpgrade()
    {
        return (int)ToStatus > (int)FromStatus;
    }

    public bool IsStatusDowngrade()
    {
        return (int)ToStatus < (int)FromStatus;
    }
}