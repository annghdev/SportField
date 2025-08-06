using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SportField.FieldService.Domain.Entities;

namespace SportField.FieldService.Infrastructure.Persistence.Configurations;

public class FieldPricingConfiguration : IEntityTypeConfiguration<FieldPricing>
{
    public void Configure(EntityTypeBuilder<FieldPricing> builder)
    {
        builder.HasKey(fp => fp.Id);

        builder.Property(fp => fp.Price)
            .IsRequired()
            .HasColumnType("decimal(18,2)");

        builder.HasOne(fp => fp.Field)
            .WithMany(f => f.FieldPricings)
            .HasForeignKey(fp => fp.FieldId);

        builder.HasOne(fp => fp.TimeSlot)
            .WithMany() // No navigation property back from TimeSlot
            .HasForeignKey(fp => fp.TimeSlotId);
    }
}
