using Common.Abstractions;
using SportField.FieldService.Domain.Entities;

namespace SportField.FieldService.Domain.Repositories
{
    public interface ITimeSlotWriteRepository : IWriteRepository<TimeSlot, string>
    {
        // No custom write methods needed for now.
    }
}
