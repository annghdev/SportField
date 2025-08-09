using Microsoft.EntityFrameworkCore;
using SportField.FieldManagement.Domain.Interfaces;
using SportField.FieldManagement.Infrastructure.Persistence;
using System.Linq.Expressions;

namespace SportField.FieldManagement.Infrastructure.Repositories;

public class FacilityRepository(FieldDbContext dbContext) : IFacilityRepository
{
    private readonly FieldDbContext _dbContext = dbContext;

    public async Task<IEnumerable<Facility>> GetAllAsync(Expression<Func<Facility, bool>> filter = null!, CancellationToken cancellationToken = default)
    {
        if (filter == null)
            return await _dbContext.Facilities.Where(e => e.DeletedDate != null).AsNoTracking().ToListAsync(cancellationToken);
        var items = _dbContext.Facilities.Where(filter).AsQueryable();
        return await items.Where(p => p.DeletedDate != null).AsNoTracking().ToListAsync(cancellationToken);
    }

    public async Task<Facility?> GetByIdAsync(Guid id)
    {
        var item = await _dbContext.Facilities.FindAsync(id);
        return item;
    }

    public async Task<Facility> AddAsync(Facility entity)
    {
        await _dbContext.AddAsync(entity);
        return entity;
    }

    public Task UpdateAsync(Facility entity)
    {
        _dbContext.Facilities.Update(entity);
        return Task.CompletedTask;
    }

    public async Task DeleteAsync(Guid id)
    {
        var item = await FindAsync(id);
        item.MarkAsDeleted();
        _dbContext.Update(item);
    }

    public async Task SetActive(Guid id, bool isActive)
    {
        var item = await FindAsync(id);
        item.SetActive(isActive);
        _dbContext.Update(item);

    }

    private async Task<Facility> FindAsync(Guid id)
    {
        var item = await _dbContext.Facilities.FindAsync(id);
        if (item == null || item.DeletedDate != null)
        {
            throw new Exception("Facility not found");
        }
        return item;
    }
}
