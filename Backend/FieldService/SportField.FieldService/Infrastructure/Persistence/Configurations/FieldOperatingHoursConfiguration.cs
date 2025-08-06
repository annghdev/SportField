using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SportField.FieldService.Domain.Entities;

namespace SportField.FieldService.Infrastructure.Persistence.Configurations;

public class FieldOperatingHoursConfiguration : IEntityTypeConfiguration<FieldOperatingHours>
{
    public void Configure(EntityTypeBuilder<FieldOperatingHours> builder)
    {
        builder.HasKey(oh => oh.Id);
        
        builder.HasOne(oh => oh.Field)
            .WithMany(f => f.OperatingHours)
            .HasForeignKey(oh => oh.FieldId);
    }
}
