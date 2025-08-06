using Common.Abstractions;
using Common.Enums;

namespace SportField.BookingService.Domain.Entities;

/// <summary>
/// Entity đại diện cho ma trận bảng lịch: intersection của (Field, TimeSlot, Date)
/// </summary>
public class CalendarSlotMatrix : AuditableEntity, IAggregateRoot
{
    public Guid FacilityId { get; set; }
    public Guid FieldId { get; set; }
    public required string TimeSlotId { get; set; }
    public DateTime Date { get; set; }
    public SlotState State { get; set; } = SlotState.Available;
    
    // Booking information (if booked)
    public Guid? BookingId { get; set; }
    public string? BookedByUserId { get; set; }
    public string? BookedByName { get; set; } // For guest bookings
    public BookingType? BookingType { get; set; }
    public BookingStatus? BookingStatus { get; set; }
    public PaymentStatus? PaymentStatus { get; set; }
    
    // Admin information
    public string? AdminNotes { get; set; }
    public bool IsBlockedByAdmin { get; set; } = false;
    public string? BlockReason { get; set; }
    
    // Lock information
    public string? LockedBySessionId { get; set; }
    public DateTime? LockedUntil { get; set; }
    
    // Pricing
    public decimal? Price { get; set; }

    // Navigation properties
    public virtual Booking? Booking { get; set; }

    public static CalendarSlotMatrix Create(Guid facilityId, Guid fieldId, string timeSlotId, DateTime date, decimal? price = null)
    {
        return new CalendarSlotMatrix
        {
            Id = Guid.CreateVersion7(),
            FacilityId = facilityId,
            FieldId = fieldId,
            TimeSlotId = timeSlotId,
            Date = date.Date,
            Price = price,
        };
    }

    public void Book(Guid bookingId, string? userId, string? bookedByName, BookingType bookingType)
    {
        State = SlotState.Booked;
        BookingId = bookingId;
        BookedByUserId = userId;
        BookedByName = bookedByName;
        BookingType = bookingType;
        BookingStatus = Common.Enums.BookingStatus.Confirmed;
        
        // Clear lock
        LockedBySessionId = null;
        LockedUntil = null;
    }

    public void Lock(string sessionId, TimeSpan lockDuration)
    {
        if (State != SlotState.Available)
            throw new InvalidOperationException($"Cannot lock slot in state: {State}");

        State = SlotState.TempLocked;
        LockedBySessionId = sessionId;
        LockedUntil = DateTime.UtcNow.Add(lockDuration);
    }

    public void Unlock()
    {
        if (State == SlotState.TempLocked)
        {
            State = IsBlockedByAdmin ? SlotState.Unavailable : SlotState.Available;
        }
        
        LockedBySessionId = null;
        LockedUntil = null;
    }

    public void Select(string sessionId)
    {
        if (State != SlotState.Available && State != SlotState.TempLocked)
            throw new InvalidOperationException($"Cannot select slot in state: {State}");

        State = SlotState.Selected;
        LockedBySessionId = sessionId;
    }

    public void CancelBooking()
    {
        State = IsBlockedByAdmin ? SlotState.Unavailable : SlotState.Available;
        BookingId = null;
        BookedByUserId = null;
        BookedByName = null;
        BookingType = null;
        BookingStatus = null;
        PaymentStatus = null;
    }

    public void BlockByAdmin(string reason, string? adminNotes = null)
    {
        if (State == SlotState.Booked)
            throw new InvalidOperationException("Cannot block a booked slot");

        State = SlotState.Unavailable;
        IsBlockedByAdmin = true;
        BlockReason = reason;
        AdminNotes = adminNotes;
    }

    public void UnblockByAdmin()
    {
        IsBlockedByAdmin = false;
        BlockReason = null;
        State = SlotState.Available;
    }

    public void UpdateBookingStatus(BookingStatus bookingStatus)
    {
        BookingStatus = bookingStatus;
    }

    public void UpdatePaymentStatus(PaymentStatus paymentStatus)
    {
        PaymentStatus = paymentStatus;
    }

    // Query methods
    public bool IsExpiredLock()
    {
        return LockedUntil.HasValue && DateTime.UtcNow > LockedUntil.Value;
    }

    public bool IsLockedBySession(string sessionId)
    {
        return LockedBySessionId == sessionId && !IsExpiredLock();
    }

    public bool IsAvailableForBooking()
    {
        return State == SlotState.Available || (State == SlotState.TempLocked && IsExpiredLock());
    }

    public bool BelongsToUser(string userId)
    {
        return BookedByUserId == userId;
    }

    public bool IsUserVisible(string? userId, bool isAdmin)
    {
        // Admin sees everything
        if (isAdmin) return true;
        
        // User sees their own bookings and available slots
        if (userId != null && BelongsToUser(userId)) return true;
        
        // Everyone sees available slots
        return IsAvailableForBooking();
    }

    public string GetDisplayInfo(bool isAdmin)
    {
        if (isAdmin)
        {
            return State switch
            {
                SlotState.Booked => $"{BookedByName ?? BookedByUserId} - {BookingStatus} - {PaymentStatus}",
                SlotState.Unavailable => BlockReason ?? "Không khả dụng",
                SlotState.TempLocked => $"Tạm khóa ({LockedBySessionId})",
                _ => State.ToString()
            };
        }
        
        return State switch
        {
            SlotState.Available => "Còn trống",
            SlotState.Booked => "Đã đặt",
            SlotState.Unavailable => "Không khả dụng",
            SlotState.TempLocked => "Tạm khóa",
            SlotState.Selected => "Đang chọn",
            _ => "Không xác định"
        };
    }

    // Composite key identifier
    public string GetSlotKey()
    {
        return $"{FieldId}_{TimeSlotId}_{Date:yyyy-MM-dd}";
    }
}