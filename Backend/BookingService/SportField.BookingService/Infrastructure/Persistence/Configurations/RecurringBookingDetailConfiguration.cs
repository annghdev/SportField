using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SportField.BookingService.Domain.Entities;

namespace SportField.BookingService.Infrastructure.Persistence.Configurations;

public class RecurringBookingDetailConfiguration : IEntityTypeConfiguration<RecurringBookingDetail>
{
    public void Configure(EntityTypeBuilder<RecurringBookingDetail> builder)
    {
        builder.HasKey(rd => rd.Id);

        builder.Property(rd => rd.BasePrice).HasColumnType("decimal(18,2)");
        builder.Property(rd => rd.DiscountPercentage).HasColumnType("decimal(5,2)");
        builder.Property(rd => rd.MonthlyAmount).HasColumnType("decimal(18,2)");

        // Storing list as JSON string
        builder.Property(rd => rd.TimeSlotIds).HasMaxLength(1000);
        builder.Property(rd => rd.DaysOfWeek).HasMaxLength(100);
    }
}
