using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SportField.BookingService.Domain.Entities;

namespace SportField.BookingService.Infrastructure.Persistence.Configurations;

public class RecurringBookingSessionConfiguration : IEntityTypeConfiguration<RecurringBookingSession>
{
    public void Configure(EntityTypeBuilder<RecurringBookingSession> builder)
    {
        builder.HasKey(s => s.Id);
        
        builder.Property(s => s.SessionAmount).HasColumnType("decimal(18,2)");
    }
}
