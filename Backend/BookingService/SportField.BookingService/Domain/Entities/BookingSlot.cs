using Common.Abstractions;

namespace SportField.BookingService.Domain.Entities;

/// <summary>
/// Slot được đặt trong Individual booking (slots rời rạc)
/// Mỗi slot đại diện cho intersection của (Field, TimeSlot, Date)
/// </summary>
public class BookingSlot : BaseEntity
{
    public Guid BookingId { get; set; }
    public Guid FieldId { get; set; } // Reference to FieldService.Field
    public required string TimeSlotId { get; set; } // Reference to FieldService.TimeSlot
    public decimal Price { get; set; }
    public bool IsActive { get; set; } = true;
    public string? Notes { get; set; }

    // Navigation properties
    public virtual Booking Booking { get; set; } = null!;

    public static BookingSlot Create(Guid bookingId, Guid fieldId, string timeSlotId, decimal price, string? notes = null)
    {
        return new BookingSlot
        {
            Id = Guid.CreateVersion7(),
            BookingId = bookingId,
            FieldId = fieldId,
            TimeSlotId = timeSlotId,
            Price = price,
            Notes = notes,
        };
    }

    public void UpdatePrice(decimal newPrice)
    {
        Price = newPrice;
    }

    public void Deactivate(string? reason = null)
    {
        IsActive = false;
        Notes = reason;
        Booking.ModifiedDate = DateTime.UtcNow;
    }

    public void Activate()
    {
        IsActive = true;
        Booking.ModifiedDate = DateTime.UtcNow;
    }
}