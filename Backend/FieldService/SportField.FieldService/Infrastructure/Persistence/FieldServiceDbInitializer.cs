using Common.Enums;
using Microsoft.EntityFrameworkCore;
using SportField.FieldService.Domain.Entities;

namespace SportField.FieldService.Infrastructure.Persistence;

public static class FieldServiceDbInitializer
{
    public static async Task InitializeAsync(FieldServiceDbContext context)
    {
        // Ensure the database is created.
        await context.Database.MigrateAsync();

        // Check if data already exists to prevent re-seeding.
        if (await context.Facilities.AnyAsync())
        {
            return; 
        }

        // --- 1. Create TimeSlots ---
        var timeSlots = new List<TimeSlot>();
        for (int hour = 5; hour < 24; hour++)
        {
            var startTime = new TimeOnly(hour, 0);
            var endTime = new TimeOnly((hour + 1) % 24, 0);
            if (hour == 23) endTime = new TimeOnly(23, 59, 59); // Avoid 00:00 for simplicity

            var timeSlot = TimeSlot.CreateWithId($"ts-{hour}", startTime, endTime);
            timeSlots.Add(timeSlot);
        }
        await context.TimeSlots.AddRangeAsync(timeSlots);

        // --- 2. Create Facilities ---
        var footballFacility = new Facility
        {
            Id = Guid.NewGuid(),
            Name = "SB Thăng Long",
            Address = "123 Đường Cầu Giấy, Hà Nội",
            OpenTime = new TimeOnly(6, 0),
            CloseTime = new TimeOnly(22, 0)
        };

        var badmintonFacility = new Facility
        {
            Id = Guid.NewGuid(),
            Name = "SCL Mỹ Đình",
            Address = "456 Đường Lê Đức Thọ, Hà Nội",
            OpenTime = new TimeOnly(5, 0),
            CloseTime = new TimeOnly(23, 59, 59)
        };
        await context.Facilities.AddRangeAsync(footballFacility, badmintonFacility);

        // --- 3. Create Fields ---
        var fields = new List<Field>
        {
            // Football Fields
            new() { Id = Guid.NewGuid(), Name = "SB 1", FacilityId = footballFacility.Id, BasePrice = 100000, Type = FieldType.Football5v5 },
            new() { Id = Guid.NewGuid(), Name = "SB 2", FacilityId = footballFacility.Id, BasePrice = 100000, Type = FieldType.Football5v5 },
            new() { Id = Guid.NewGuid(), Name = "SB 3 (VIP)", FacilityId = footballFacility.Id, BasePrice = 150000, Type = FieldType.Football7v7 },
            new() { Id = Guid.NewGuid(), Name = "SB 4 (VIP)", FacilityId = footballFacility.Id, BasePrice = 150000, Type = FieldType.Football7v7 },
            // Badminton Fields
            new() { Id = Guid.NewGuid(), Name = "SCL 1", FacilityId = badmintonFacility.Id, BasePrice = 100000, Type = FieldType.Badminton },
            new() { Id = Guid.NewGuid(), Name = "SCL 2", FacilityId = badmintonFacility.Id, BasePrice = 100000, Type = FieldType.Badminton },
            new() { Id = Guid.NewGuid(), Name = "SCL 3 (VIP)", FacilityId = badmintonFacility.Id, BasePrice = 150000, Type = FieldType.Badminton },
            new() { Id = Guid.NewGuid(), Name = "SCL 4 (VIP)", FacilityId = badmintonFacility.Id, BasePrice = 150000, Type = FieldType.Badminton }
        };
        await context.Fields.AddRangeAsync(fields);

        // --- 4. Create Operating Hours, Pricing, and Maintenance for each field ---
        var allOperatingHours = new List<FieldOperatingHours>();
        var allPricings = new List<FieldPricing>();
        var allMaintenances = new List<FieldMaintenance>();
        var afternoonStart = new TimeOnly(12, 0);

        foreach (var field in fields)
        {
            var facility = field.FacilityId == footballFacility.Id ? footballFacility : badmintonFacility;

            // Operating Hours
            for (int i = 0; i < 7; i++)
            {
                var day = (DayOfWeek)i;
                var openTime = facility.OpenTime;
                if (day == DayOfWeek.Monday) openTime = openTime.AddHours(1);

                allOperatingHours.Add(new FieldOperatingHours { Id = Guid.NewGuid(), FieldId = field.Id, DayOfWeek = day, OpenTime = openTime, CloseTime = facility.CloseTime });
            }

            // Pricing Rules
            foreach (var slot in timeSlots)
            {
                // Afternoon price increase
                if (slot.StartTime >= afternoonStart)
                {
                    allPricings.Add(new FieldPricing { Id = Guid.NewGuid(), FieldId = field.Id, TimeSlotId = slot.Id, Price = field.BasePrice + 10000 });
                }
                // Weekend price increase
                allPricings.Add(new FieldPricing { Id = Guid.NewGuid(), FieldId = field.Id, TimeSlotId = slot.Id, Price = field.BasePrice + 20000, DayOfWeek = DayOfWeek.Saturday });
                allPricings.Add(new FieldPricing { Id = Guid.NewGuid(), FieldId = field.Id, TimeSlotId = slot.Id, Price = field.BasePrice + 20000, DayOfWeek = DayOfWeek.Sunday });
                
                // Weekend Afternoon price increase (most specific rule)
                if (slot.StartTime >= afternoonStart)
                {
                     allPricings.Add(new FieldPricing { Id = Guid.NewGuid(), FieldId = field.Id, TimeSlotId = slot.Id, Price = field.BasePrice + 10000 + 20000, DayOfWeek = DayOfWeek.Saturday });
                     allPricings.Add(new FieldPricing { Id = Guid.NewGuid(), FieldId = field.Id, TimeSlotId = slot.Id, Price = field.BasePrice + 10000 + 20000, DayOfWeek = DayOfWeek.Sunday });
                }
            }

            // Maintenance
            var maintenanceStartTime = facility.OpenTime.AddHours(1); // Monday open time
            var maintenanceEndTime = maintenanceStartTime.AddHours(1);
            allMaintenances.Add(new FieldMaintenance
            {
                Id = Guid.NewGuid(),
                FieldId = field.Id,
                Title = "Bảo trì định kỳ đầu tuần",
                Description = "Kiểm tra và dọn dẹp sân định kỳ.",
                StartTime = DateTime.UtcNow.Date.AddDays(DayOfWeek.Monday - DateTime.UtcNow.DayOfWeek).Add(maintenanceStartTime.ToTimeSpan()),
                EndTime = DateTime.UtcNow.Date.AddDays(DayOfWeek.Monday - DateTime.UtcNow.DayOfWeek).Add(maintenanceEndTime.ToTimeSpan()),
                IsRecurring = true,
                RecurrencePattern = "{\"interval\":\"weekly\",\"dayOfWeek\":1}", // JSON for weekly on Monday
                Status = MaintenanceStatus.Scheduled,
                Type = MaintenanceType.Cleaning
            });
        }
        await context.FieldOperatingHours.AddRangeAsync(allOperatingHours);
        await context.FieldPricings.AddRangeAsync(allPricings);
        await context.FieldMaintenances.AddRangeAsync(allMaintenances);

        // --- Save all changes to the database ---
        await context.SaveChangesAsync();
    }
}
