using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SportField.BookingService.Domain.Entities;

namespace SportField.BookingService.Infrastructure.Persistence.Configurations;

public class RecurringBookingScheduleConfiguration : IEntityTypeConfiguration<RecurringBookingSchedule>
{
    public void Configure(EntityTypeBuilder<RecurringBookingSchedule> builder)
    {
        builder.HasKey(s => s.Id);
        
        builder.Property(s => s.MonthlyAmount).HasColumnType("decimal(18,2)");
    }
}
