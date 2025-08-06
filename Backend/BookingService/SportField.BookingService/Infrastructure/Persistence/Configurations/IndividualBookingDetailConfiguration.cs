using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SportField.BookingService.Domain.Entities;

namespace SportField.BookingService.Infrastructure.Persistence.Configurations;

public class IndividualBookingDetailConfiguration : IEntityTypeConfiguration<IndividualBookingDetail>
{
    public void Configure(EntityTypeBuilder<IndividualBookingDetail> builder)
    {
        builder.HasKey(id => id.Id);
    }
}
