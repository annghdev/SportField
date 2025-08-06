using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SportField.FieldService.Domain.Entities;

namespace SportField.FieldService.Infrastructure.Persistence.Configurations;

public class FieldMaintenanceConfiguration : IEntityTypeConfiguration<FieldMaintenance>
{
    public void Configure(EntityTypeBuilder<FieldMaintenance> builder)
    {
        builder.HasKey(m => m.Id);

        builder.Property(m => m.Title)
            .IsRequired()
            .HasMaxLength(250);

        builder.Property(m => m.EstimatedCost)
            .HasColumnType("decimal(18,2)");
        
        builder.Property(m => m.ActualCost)
            .HasColumnType("decimal(18,2)");
            
        builder.HasOne(m => m.Field)
            .WithMany(f => f.MaintenanceSchedules)
            .HasForeignKey(m => m.FieldId);
    }
}
