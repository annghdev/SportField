using Common.Abstractions;
using SportField.FieldService.Domain;
using SportField.FieldService.Domain.Entities;

namespace SportField.FieldService.Domain.Repositories
{
    public interface IFieldWriteRepository : IWriteRepository<Field, Guid>
    {
        // No custom write methods needed for now.
    }
}
