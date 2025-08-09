using SportField.FieldManagement.Domain.Interfaces;
using SportField.FieldManagement.Infrastructure.Persistence;

namespace SportField.FieldManagement.Infrastructure.Repositories;

public class TimeFrameRepository(FieldDbContext dbContext) : ITimeFrameRepository
{
    private readonly FieldDbContext _dbContext = dbContext;

    public async Task SetActive(string id, bool active)
    {
        var item = await _dbContext.TimeFrames.FindAsync(id) ?? throw new NotFoundException("TimeFrame", id);
        item.SetActive(active);
        _dbContext.TimeFrames.Update(item);
    }
}
