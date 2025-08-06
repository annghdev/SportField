using Microsoft.EntityFrameworkCore;
using SportField.BookingService.Application.ReadModels;
using SportField.BookingService.Domain.Entities;
using System.Reflection;

namespace SportField.BookingService.Infrastructure.Persistence;

public class BookingServiceDbContext : DbContext
{
    public BookingServiceDbContext(DbContextOptions<BookingServiceDbContext> options) : base(options) { }

    // Domain Aggregates
    public DbSet<Booking> Bookings => Set<Booking>();
    public DbSet<BookingPayment> BookingPayments => Set<BookingPayment>();
    public DbSet<CalendarSlotMatrix> CalendarSlotMatrix => Set<CalendarSlotMatrix>();
    public DbSet<SlotLock> SlotLocks => Set<SlotLock>();
    
    // Read Models
    public DbSet<FieldProjection> FieldProjections => Set<FieldProjection>();
    public DbSet<TimeSlotProjection> TimeSlotProjections => Set<TimeSlotProjection>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        // Apply configurations for domain entities
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        // Configure read models separately
        modelBuilder.Entity<FieldProjection>().ToTable("FieldProjections_Read");
        modelBuilder.Entity<TimeSlotProjection>().ToTable("TimeSlotProjections_Read");
    }
}
