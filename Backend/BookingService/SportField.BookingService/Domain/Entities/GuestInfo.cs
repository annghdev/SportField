using Common.Abstractions;

namespace SportField.BookingService.Domain.Entities;

/// <summary>
/// Thông tin khách vãng lai cho Individual booking
/// </summary>
public class GuestInfo : BaseEntity
{
    public Guid BookingId { get; set; }
    public required string FullName { get; set; }
    public required string PhoneNumber { get; set; }
    public string? Email { get; set; }
    public string? Notes { get; set; }
    public DateTime? UpdatedDate { get; set; }

    // Navigation property
    public virtual Booking Booking { get; set; } = null!;

    public static GuestInfo Create(Guid bookingId, string fullName, string phoneNumber, 
        string? email = null, string? notes = null)
    {
        return new GuestInfo
        {
            Id = Guid.CreateVersion7(),
            BookingId = bookingId,
            FullName = fullName.Trim(),
            PhoneNumber = phoneNumber.Trim(),
            Email = email?.Trim(),
            Notes = notes?.Trim(),
        };
    }

    public void UpdateInfo(string fullName, string phoneNumber, string? email = null, string? notes = null)
    {
        FullName = fullName.Trim();
        PhoneNumber = phoneNumber.Trim();
        Email = email?.Trim();
        Notes = notes?.Trim();
        UpdatedDate = DateTime.UtcNow;
    }
}