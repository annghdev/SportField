using Common.Abstractions;

namespace SportField.BookingService.Domain.Entities;

/// <summary>
/// Chi tiết cho Individual booking - chọn các slots rời rạc
/// </summary>
public class IndividualBookingDetail : BaseEntity
{
    public Guid BookingId { get; set; }
    
    // Navigation properties
    public virtual Booking Booking { get; set; } = null!;
    public virtual ICollection<BookingSlot> BookingSlots { get; set; } = [];

    public static IndividualBookingDetail Create(Guid bookingId)
    {
        return new IndividualBookingDetail
        {
            Id = Guid.CreateVersion7(),
            BookingId = bookingId,
        };
    }

    public void AddSlot(Guid fieldId, string timeSlotId, decimal price)
    {
        var bookingSlot = BookingSlot.Create(BookingId, fieldId, timeSlotId, price);
        BookingSlots.Add(bookingSlot);
    }

    public void RemoveSlot(Guid fieldId, string timeSlotId)
    {
        var slot = BookingSlots.FirstOrDefault(bs => bs.FieldId == fieldId && bs.TimeSlotId == timeSlotId);
        if (slot != null)
        {
            BookingSlots.Remove(slot);
        }
    }

    public void RemoveSlotById(Guid slotId)
    {
        var slot = BookingSlots.FirstOrDefault(bs => bs.Id == slotId);
        if (slot != null)
        {
            BookingSlots.Remove(slot);
        }
    }

    public decimal GetTotalAmount()
    {
        return BookingSlots.Where(bs => bs.IsActive).Sum(bs => bs.Price);
    }

    public List<string> GetActiveTimeSlotIds()
    {
        return BookingSlots.Where(bs => bs.IsActive).Select(bs => bs.TimeSlotId).ToList();
    }

    public List<Guid> GetActiveFieldIds()
    {
        return BookingSlots.Where(bs => bs.IsActive).Select(bs => bs.FieldId).Distinct().ToList();
    }

    public List<(Guid FieldId, string TimeSlotId)> GetActiveSlots()
    {
        return BookingSlots.Where(bs => bs.IsActive)
            .Select(bs => (bs.FieldId, bs.TimeSlotId))
            .ToList();
    }
}