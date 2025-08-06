using Microsoft.EntityFrameworkCore;
using SportField.FieldService.Domain.Entities;
using System.Reflection;

namespace SportField.FieldService.Infrastructure.Persistence;

public class FieldServiceDbContext : DbContext
{
    public FieldServiceDbContext(DbContextOptions<FieldServiceDbContext> options) : base(options) { }

    public DbSet<Facility> Facilities => Set<Facility>();
    public DbSet<Field> Fields => Set<Field>();
    public DbSet<TimeSlot> TimeSlots => Set<TimeSlot>();
    public DbSet<FieldPricing> FieldPricings => Set<FieldPricing>();
    public DbSet<FieldOperatingHours> FieldOperatingHours => Set<FieldOperatingHours>();
    public DbSet<FieldMaintenance> FieldMaintenances => Set<FieldMaintenance>();
    public DbSet<FieldAvailability> FieldAvailabilities => Set<FieldAvailability>();


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}
