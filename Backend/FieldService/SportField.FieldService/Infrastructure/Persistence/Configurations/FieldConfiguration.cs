using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SportField.FieldService.Domain.Entities;

namespace SportField.FieldService.Infrastructure.Persistence.Configurations;

public class FieldConfiguration : IEntityTypeConfiguration<Field>
{
    public void Configure(EntityTypeBuilder<Field> builder)
    {
        builder.HasKey(f => f.Id);

        builder.Property(f => f.Name)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(f => f.BasePrice)
            .HasColumnType("decimal(18,2)");
        
        // A Field belongs to one Facility
        builder.HasOne(f => f.Facility)
            .WithMany(fa => fa.Fields)
            .HasForeignKey(f => f.FacilityId)
            .OnDelete(DeleteBehavior.Restrict); // Prevent deleting a facility if it has fields

        // One-to-many relationships with dependent entities
        builder.HasMany(f => f.FieldPricings)
            .WithOne(p => p.Field)
            .HasForeignKey(p => p.FieldId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(f => f.OperatingHours)
            .WithOne(o => o.Field)
            .HasForeignKey(o => o.FieldId)
            .OnDelete(DeleteBehavior.Cascade);
            
        builder.HasMany(f => f.MaintenanceSchedules)
            .WithOne(m => m.Field)
            .HasForeignKey(m => m.FieldId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(f => f.BlockedAvailabilities)
            .WithOne(b => b.Field)
            .HasForeignKey(b => b.FieldId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
