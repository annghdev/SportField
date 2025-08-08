using Common.Abstractions;
using SportField.IdentityService.Domain.Entities;

namespace SportField.IdentityService.Domain.Repositories;

public interface IUserWriteRepository : IWriteRepository<User, Guid>
{
    Task<User> CreateAsync(User user, CancellationToken cancellationToken = default);
    Task UpdateAsync(User user, CancellationToken cancellationToken = default);
    Task DeleteAsync(Guid userId, CancellationToken cancellationToken = default);
    Task SoftDeleteAsync(Guid userId, CancellationToken cancellationToken = default);
}
