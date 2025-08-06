using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SportField.BookingService.Domain.Entities;

namespace SportField.BookingService.Infrastructure.Persistence.Configurations;

public class BookingConfiguration : IEntityTypeConfiguration<Booking>
{
    public void Configure(EntityTypeBuilder<Booking> builder)
    {
        builder.HasKey(b => b.Id);

        builder.Property(b => b.BaseAmount).HasColumnType("decimal(18,2)");
        builder.Property(b => b.DiscountAmount).HasColumnType("decimal(18,2)");
        builder.Property(b => b.TotalAmount).HasColumnType("decimal(18,2)");
    }
}
