using SportField.FieldManagement.Domain.Interfaces;
using SportField.FieldManagement.Infrastructure.Persistence;
using SportField.FieldManagement.Infrastructure.Repositories;

namespace SportField.FieldManagement.Infrastructure;

public class FieldUnitOfWork(FieldDbContext dbContext) : IFieldUnitOfWork
{
    private readonly FieldDbContext _dbContext = dbContext;
    private IFacilityRepository? facilityRepository; 
    private IFieldRepository? fieldRepository; 
    private ITimeFrameRepository? timeFrameRepository;

    public IFacilityRepository FacilityRepository => facilityRepository ??= new FacilityRepository(_dbContext);
    public IFieldRepository FieldRepository => fieldRepository ??= new FieldRepository(_dbContext);
    public ITimeFrameRepository TimeFrameRepository  => timeFrameRepository ??= new TimeFrameRepository(_dbContext);

    public Task BeginAsync(CancellationToken cancellationToken = default)
    {
        return Task.CompletedTask;
    }

    public async Task CommitAsync(CancellationToken cancellationToken = default)
    {
        await _dbContext.SaveChangesAsync(cancellationToken);
        // Add events dispatcher here
    }
}
