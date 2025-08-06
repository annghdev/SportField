using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SportField.BookingService.Domain.Entities;

namespace SportField.BookingService.Infrastructure.Persistence.Configurations
{
    public class BookingStatusHistoryConfiguration : IEntityTypeConfiguration<BookingStatusHistory>
    {
        public void Configure(EntityTypeBuilder<BookingStatusHistory> builder)
        {
            builder.HasKey(x => x.Id);
        }
    }
}
