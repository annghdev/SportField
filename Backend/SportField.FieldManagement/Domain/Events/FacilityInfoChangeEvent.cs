using SportField.SharedKernel.DomainBase;

namespace SportField.FieldManagement.Domain.Events
{
    public record FacilityInfoChangeEvent(
        Guid FacilityId,
        string? Name,
        string? Description,
        string? Address,
        string? Location,
        string? ImgUrls
        ): BaseDomainEvent
    {
    }
}
