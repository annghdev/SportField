using Microsoft.EntityFrameworkCore;
using SportField.FieldManagement.Infrastructure.Persistence;
using System.Linq.Expressions;

namespace SportField.FieldManagement.Infrastructure.Repositories;

public class GenericRepository<T, K>(FieldDbContext dbContext) : IRepository<T, K>
    where T : class, IAggregateRoot
{
    private readonly FieldDbContext _dbContext = dbContext;

    public async Task<T> AddAsync(T entity)
    {
        await _dbContext.Set<T>().AddAsync(entity);
        return entity;
    }
    public Task UpdateAsync(T entity)
    {
        _dbContext.Set<T>().Update(entity);
        return Task.CompletedTask;

    }
    public async Task DeleteAsync(K id)
    {
        var entity = await _dbContext.Set<T>().FindAsync(id) ?? throw new InvalidOperationException("Entity not found!");
        _dbContext.Set<T>().Remove(entity);
    }

    public virtual async Task<IEnumerable<T>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _dbContext.Set<T>().ToListAsync(cancellationToken);
    }

    public virtual async Task<T?> GetByIdAsync(K id)
    {
        return await _dbContext.Set<T>().FindAsync(id);
    }
}
