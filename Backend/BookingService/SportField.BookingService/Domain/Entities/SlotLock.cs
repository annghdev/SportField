using Common.Abstractions;
using Common.Enums;

namespace SportField.BookingService.Domain.Entities;

/// <summary>
/// Quản lý khóa tạm thời slot trong quá trình booking (real-time conflict prevention)
/// </summary>
public class SlotLock : BaseEntity
{
    public Guid FieldId { get; set; }
    public required string TimeSlotId { get; set; }
    public DateTime BookingDate { get; set; }
    public string? SessionId { get; set; } // Browser session hoặc user session
    public string? UserId { get; set; }
    public DateTime LockedAt { get; set; }
    public DateTime ExpiresAt { get; set; }
    public SlotState LockReason { get; set; } = SlotState.TempLocked;
    public bool IsActive { get; set; } = true;

    public static SlotLock CreateTempLock(Guid fieldId, string timeSlotId, DateTime bookingDate, 
        string sessionId, TimeSpan lockDuration, string? userId = null)
    {
        var now = DateTime.UtcNow;
        return new SlotLock
        {
            Id = Guid.CreateVersion7(),
            FieldId = fieldId,
            TimeSlotId = timeSlotId,
            BookingDate = bookingDate.Date,
            SessionId = sessionId,
            UserId = userId,
            LockedAt = now,
            ExpiresAt = now.Add(lockDuration),
        };
    }

    public void ExtendLock(TimeSpan additionalTime)
    {
        ExpiresAt = ExpiresAt.Add(additionalTime);
    }

    public void ReleaseLock()
    {
        IsActive = false;
    }

    public bool IsExpired()
    {
        return DateTime.UtcNow > ExpiresAt;
    }

    public TimeSpan GetRemainingTime()
    {
        var remaining = ExpiresAt - DateTime.UtcNow;
        return remaining > TimeSpan.Zero ? remaining : TimeSpan.Zero;
    }

    public bool BelongsToSession(string sessionId)
    {
        return SessionId == sessionId;
    }

    public bool BelongsToUser(string userId)
    {
        return UserId == userId;
    }
}