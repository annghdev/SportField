namespace SportField.FieldManagement.Domain.Interfaces;

public interface IFieldUnitOfWork : IUnitOfWork
{
     IFacilityRepository FacilityRepository { get;}
     IFieldRepository FieldRepository { get; }
     ITimeFrameRepository TimeFrameRepository { get; }
}
