using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SportField.FieldService.Domain.Entities;

namespace SportField.FieldService.Infrastructure.Persistence.Configurations;

public class FacilityConfiguration : IEntityTypeConfiguration<Facility>
{
    public void Configure(EntityTypeBuilder<Facility> builder)
    {
        builder.HasKey(f => f.Id);
        
        builder.Property(f => f.Name)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(f => f.Address)
            .IsRequired()
            .HasMaxLength(500);
            
        builder.Property(f => f.PhoneNumber)
            .HasMaxLength(20);

        builder.Property(f => f.Email)
            .HasMaxLength(100);

        // A Facility can have multiple Fields
        builder.HasMany(f => f.Fields)
            .WithOne(fi => fi.Facility)
            .HasForeignKey(fi => fi.FacilityId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
