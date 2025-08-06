using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SportField.BookingService.Domain.Entities;

namespace SportField.BookingService.Infrastructure.Persistence.Configurations;

public class CalendarSlotMatrixConfiguration : IEntityTypeConfiguration<CalendarSlotMatrix>
{
    public void Configure(EntityTypeBuilder<CalendarSlotMatrix> builder)
    {
        builder.HasKey(c => c.Id);

        // To enforce uniqueness for the combination of fields that define a slot
        builder.HasIndex(c => new { c.FacilityId, c.FieldId, c.TimeSlotId, c.Date })
            .IsUnique();
        
        builder.Property(c => c.Price).HasColumnType("decimal(18,2)");
    }
}
