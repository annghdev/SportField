namespace SportField.FieldManagement.Domain.Interfaces;

public interface IFieldUnitOfWork : IUnitOfWork
{
     IFacilityRepository FacilityRepository { get; set; }
     IFieldRepository FieldRepository { get; set; }
}
