using Common.Abstractions;
using Common.Enums;

namespace SportField.BookingService.Domain.Entities;

/// <summary>
/// Entity chính đại diện cho một đơn đặt sân (Individual hoặc Recurring)
/// </summary>
public class Booking : AuditableEntity, IAggregateRoot
{
    // Thông tin cơ bản
    public BookingType Type { get; set; }
    public BookingOrigin Origin { get; set; }
    public BookingStatus Status { get; set; } = BookingStatus.Pending;
    public Guid FacilityId { get; set; }
    
    // Thông tin người đặt
    public Guid? UserId { get; set; } // null cho guest booking
    public Guid? CreatedByAdminId { get; set; } // Admin đặt hộ
    
    // Thông tin tài chính
    public decimal BaseAmount { get; set; }
    public decimal DiscountAmount { get; set; } = 0;
    public decimal TotalAmount { get; set; }
    
    // Thông tin thời gian
    public DateTime BookingDate { get; set; } // Cho Individual booking
    public DateTime? ConfirmedDate { get; set; }
    public DateTime? CancelledDate { get; set; }
    public string? CancellationReason { get; set; }
    
    // Ghi chú
    public string? Notes { get; set; }
    public string? AdminNotes { get; set; } // Ghi chú nội bộ của admin
    
    // Navigation properties
    public virtual GuestInfo? GuestInfo { get; set; }
    public virtual IndividualBookingDetail? IndividualDetail { get; set; }
    public virtual RecurringBookingDetail? RecurringDetail { get; set; }
    public virtual ICollection<BookingPayment> Payments { get; set; } = [];
    public virtual ICollection<BookingStatusHistory> StatusHistory { get; set; } = [];

    // Factory methods
    public static Booking CreateIndividualBooking(
        Guid facilityId, DateTime bookingDate,
        Guid? userId = null, Guid? createdByAdminId = null)
    {
        return new Booking
        {
            Id = Guid.CreateVersion7(),
            Type = BookingType.Individual,
            Origin = DetermineOrigin(userId, createdByAdminId),
            FacilityId = facilityId,
            BookingDate = bookingDate,
            UserId = userId,
            CreatedByAdminId = createdByAdminId,
            CreatedDate = DateTime.UtcNow
        };
    }

    public static Booking CreateRecurringBooking(
        Guid facilityId, Guid userId,
        Guid? createdByAdminId = null)
    {
        return new Booking
        {
            Id = Guid.CreateVersion7(),
            Type = BookingType.Recurring,
            Origin = createdByAdminId != null ? BookingOrigin.AdminBooking : BookingOrigin.CustomerDirect,
            FacilityId = facilityId,
            UserId = userId,
            CreatedByAdminId = createdByAdminId,
            CreatedDate = DateTime.UtcNow
        };
    }

    private static BookingOrigin DetermineOrigin(Guid? userId, Guid? createdByAdminId)
    {
        if (createdByAdminId != null) return BookingOrigin.AdminBooking;
        if (userId == null) return BookingOrigin.GuestBooking;
        return BookingOrigin.CustomerDirect;
    }

    // Business methods
    public void Confirm()
    {
        Status = BookingStatus.Confirmed;
        ConfirmedDate = DateTime.UtcNow;
        ModifiedDate = DateTime.UtcNow;
        
        AddStatusHistory(BookingStatus.Confirmed, "Booking confirmed");
    }

    public void Cancel(string reason, Guid? cancelledByUserId = null)
    {
        Status = BookingStatus.Cancelled;
        CancelledDate = DateTime.UtcNow;
        CancellationReason = reason;
        ModifiedDate = DateTime.UtcNow;
        
        AddStatusHistory(BookingStatus.Cancelled, reason, cancelledByUserId);
    }

    public void UpdateTotalAmount(decimal baseAmount, decimal discountAmount = 0)
    {
        BaseAmount = baseAmount;
        DiscountAmount = discountAmount;
        TotalAmount = baseAmount - discountAmount;
        ModifiedDate = DateTime.UtcNow;
    }

    private void AddStatusHistory(BookingStatus status, string? notes = null, Guid? changedByUserId = null)
    {
        StatusHistory.Add(BookingStatusHistory.Create(Id, status, notes, changedByUserId));
    }

    public bool CanBeCancelled()
    {
        return Status == BookingStatus.Pending || Status == BookingStatus.Confirmed;
    }

    public bool IsGuestBooking()
    {
        return Origin == BookingOrigin.GuestBooking;
    }

    public bool IsAdminBooking()
    {
        return Origin == BookingOrigin.AdminBooking;
    }
}