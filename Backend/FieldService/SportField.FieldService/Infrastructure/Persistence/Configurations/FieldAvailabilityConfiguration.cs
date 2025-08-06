using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SportField.FieldService.Domain.Entities;

namespace SportField.FieldService.Infrastructure.Persistence.Configurations;

public class FieldAvailabilityConfiguration : IEntityTypeConfiguration<FieldAvailability>
{
    public void Configure(EntityTypeBuilder<FieldAvailability> builder)
    {
        builder.HasKey(fa => fa.Id);

        builder.HasOne(fa => fa.Field)
            .WithMany(f => f.BlockedAvailabilities)
            .HasForeignKey(fa => fa.FieldId);
        
        builder.HasOne(fa => fa.TimeSlot)
            .WithMany()
            .HasForeignKey(fa => fa.TimeSlotId);
    }
}
