namespace SportField.FieldManagement.Domain.Interfaces
{
    public interface ITimeFrameRepository
    {
        Task SetActive(string id, bool active);
    }
}
