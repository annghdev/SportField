namespace SportField.FieldManagement.Domain.Entities
{
    public class FieldPricing : BaseEntity
    {
        public Guid FieldId { get; set; }
        public required string TimeFrameId { get; set; }
        public decimal Price { get; set; }
        public string? DayOfWeek { get; set; } // null khi áp dụng tất cả các ngày trong tuần
        public DateTime? EffectiveFrom { get; set; } // thời gian áp dụng khi có sự kiện khuyến mãi
        public DateTime? EffectiveTo { get; set; }

        // Navigation properties
        public virtual Field? Field { get; set; }
        public virtual TimeFrame? TimeFrame { get; set; }

        // Factory method
        public static FieldPricing Create(
            Guid fieldId,
            string timeFrameId,
            decimal price,
            string? dayOfWeek,
            DateTime? effectiveFrom,
            DateTime? effectiveTo
            )
        {
            if (price < 0)
                throw new PriceLessThanZeroException();
            var fieldPricing = new FieldPricing
            {
                FieldId = fieldId,
                TimeFrameId = timeFrameId,
                Price = price,
                DayOfWeek = dayOfWeek,
                EffectiveFrom = effectiveFrom,
                EffectiveTo = effectiveTo
            };
            fieldPricing.AddDomainEvent(new FieldPricingAddEvent(fieldId, fieldPricing.Id, timeFrameId, price, effectiveFrom, effectiveTo));
            return fieldPricing;
        }
        public void UpdatePrice(
            decimal price)
        {
            if (price < 0)
                throw new PriceLessThanZeroException();
            Price = price;
            MarkAsModified();
            AddDomainEvent(new FieldPricingChangeEvent(FieldId, Id, price));
        }
        public void ExtendEffectiveDate(DateTime effectiveTo)
        {
            if (EffectiveFrom == null)
                throw new PricingEffectiveDateException(Id);
            EffectiveTo = effectiveTo;
            MarkAsModified();
        }
        public void MarkAsDeleted()
        {
            MarkAsModified();
        }
        private void MarkAsModified()
        {
            if (Field != null)
                Field.ModifiedDate = DateTime.UtcNow;
        }
    }
}
