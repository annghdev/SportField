using Microsoft.EntityFrameworkCore;
using SportField.FieldManagement.Application.ReadModels;
using System.Reflection;

namespace SportField.FieldManagement.Infrastructure.Persistence;

public class FieldDbContext : DbContext
{
    public FieldDbContext(DbContextOptions<FieldDbContext> options) : base(options)
    {
    }
    public virtual DbSet<Facility> Facilities { get; set; }
    public virtual DbSet<Field> Fields { get; set; }
    public virtual DbSet<FieldPricing> FieldPricings { get; set; }
    public virtual DbSet<TimeSlotProjection> TimeSlotProjections { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}
