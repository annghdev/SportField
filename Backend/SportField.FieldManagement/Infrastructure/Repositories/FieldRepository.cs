using Microsoft.EntityFrameworkCore;
using SportField.FieldManagement.Domain.Interfaces;
using SportField.FieldManagement.Infrastructure.Persistence;
using System.Linq;
using System.Linq.Expressions;

namespace SportField.FieldManagement.Infrastructure.Repositories;

public class FieldRepository(FieldDbContext dbContext) : IFieldRepository
{
    private readonly FieldDbContext _dbContext = dbContext;

    public async Task<IEnumerable<Field>> GetAllAsync(Expression<Func<Field, bool>> filter = null!, CancellationToken cancellationToken = default)
    {
        if (filter == null)
            return await _dbContext.Fields.Where(e => e.DeletedDate != null).AsNoTracking().ToListAsync(cancellationToken);
        var items = _dbContext.Fields.Where(filter).AsQueryable();
        return await items.Where(p => p.DeletedDate != null).AsNoTracking().ToListAsync(cancellationToken);
    }
    public async Task<Field?> GetByIdAsync(Guid id)
    {
        var item = await _dbContext.Fields
            .Include(f => f.FieldPricings)
                .ThenInclude(p => p.TimeFrame)
            .Include(f => f.FieldMaintenances)
                .ThenInclude(m => m.TimeFrame)
            .FirstOrDefaultAsync(e => e.Id == id);
        return item;
    }
    public async Task<Field> AddAsync(Field entity)
    {
        await _dbContext.Fields.AddAsync(entity);
        return entity;
    }
    public async Task SetActiveAsync(Guid id, bool isActive)
    {
        var item = await FindFieldAsync(id);
        item.SetActive(isActive);

    }
    public Task UpdateAsync(Field entity)
    {
        throw new NotImplementedException();
    }
    public Task DeleteAsync(Guid id)
    {
        throw new NotImplementedException();
    }

    public Task AddPricingAsync(FieldPricing pricing)
    {
        throw new NotImplementedException();
    }

    public Task UpdatePricing(Dictionary<Guid, decimal> pricings)
    {
        throw new NotImplementedException();
    }
    public Task ExtendPricingEffectiveDateAsync(IEnumerable<Guid> pricingIds, DateTime effectiveTo)
    {
        throw new NotImplementedException();
    }
    public Task DeletePricing(Guid pricingId)
    {
        throw new NotImplementedException();
    }


    public Task AddMaintenance(FieldMaintenance maintenance)
    {
        throw new NotImplementedException();
    }
    public Task SetMaintenanceActive(Guid maintenanceId, bool isActive)
    {
        throw new NotImplementedException();
    }
    public Task DeleteMaintenance(Guid maintenanceId)
    {
        throw new NotImplementedException();
    }

    private async Task<Field> FindFieldAsync(Guid id)
    {
        var item = await _dbContext.Fields.FindAsync(id);
        if (item == null || item.DeletedDate != null)
        {
            throw new NotFoundException("Field", id.ToString());
        }
        return item;
    }
    private async Task<FieldPricing> FindPricingAsync(Guid id)
    {
        var item = await _dbContext.FieldPricings.FindAsync(id);
        return item ?? throw new NotFoundException("FieldPricing", id.ToString());
    }
    private async Task<FieldMaintenance> FindMaintenanceAsync(Guid id)
    {
        var item = await _dbContext.FieldMaintenances.FindAsync(id);
        if (item == null || item.DeletedDate != null)
        {
            throw new NotFoundException("FieldMaintenance", id.ToString());
        }
        return item;
    }
}
