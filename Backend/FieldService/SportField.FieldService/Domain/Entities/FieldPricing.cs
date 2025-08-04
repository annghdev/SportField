using Common.Abstractions;
using SportField.FieldService.Domain.Events;

namespace SportField.FieldService.Domain.Entities;

public class FieldPricing : BaseEntity
{
    public required string FieldId { get; set; }
    public required string TimeSlotId { get; set; }
    public decimal Price { get; set; }
    public DayOfWeek? DayOfWeek { get; set; } // null = áp dụng cho tất cả các ngày
    public DateTime? EffectiveFrom { get; set; }
    public DateTime? EffectiveTo { get; set; }
    public bool IsActive { get; set; } = true;

    // Navigation properties
    public virtual Field Field { get; set; } = null!;
    public virtual TimeSlot TimeSlot { get; set; } = null!;

    public static FieldPricing Create(string FieldId, string timeSlotId, decimal price,
        DayOfWeek? dayOfWeek = null, DateTime? effectiveFrom = null, DateTime? effectiveTo = null)
    {
        var pricing = new FieldPricing
        {
            FieldId = FieldId,
            TimeSlotId = timeSlotId,
            Price = price,
            DayOfWeek = dayOfWeek,
            EffectiveFrom = effectiveFrom,
            EffectiveTo = effectiveTo
        };

        pricing.AddDomainEvent(new TimeSlotPriceUpdatedEvent(FieldId, timeSlotId, price, dayOfWeek));
        return pricing;
    }

    public void UpdatePrice(decimal newPrice)
    {
        Price = newPrice;

        AddDomainEvent(new TimeSlotPriceUpdatedEvent(FieldId, TimeSlotId, newPrice, DayOfWeek));
    }

    public bool IsValidForDate(DateTime date)
    {
        // Kiểm tra ngày trong tuần nếu có
        if (DayOfWeek.HasValue && date.DayOfWeek != DayOfWeek.Value)
            return false;

        // Kiểm tra thời gian hiệu lực
        if (EffectiveFrom.HasValue && date < EffectiveFrom.Value)
            return false;

        if (EffectiveTo.HasValue && date > EffectiveTo.Value)
            return false;

        return IsActive;
    }
}