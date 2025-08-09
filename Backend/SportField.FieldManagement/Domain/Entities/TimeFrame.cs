namespace SportField.FieldManagement.Domain.Entities;

/// <summary>
/// Các khung giờ cố định của hoạt động sân
/// Được khởi tạo tĩnh, không thể sửa/xóa
/// </summary>
public class TimeFrame : BaseEntity<string>, IAggregateRoot
{
    public TimeOnly StartTime { get; set; }
    public TimeOnly EndTime { get; set; }
    public DateTime? ModifiedDate { get; set; }
    public string? ModifiedBy { get; set; }
    public int Numering { get; set; } // số thứ tự để dễ validate khung giờ liên tục
    public int DurationInMinutes { get; set; }
    public bool IsActive { get; set; }
    public string Display => EndTime.Hour == 23 && EndTime.Minute == 59 ?
        $"{StartTime:HH:mm} - 00:00" : $"{StartTime:HH:mm} - {EndTime:HH:mm}";

    public void SetActive(bool isActive)
    {
        IsActive = isActive;
        ModifiedDate = DateTime.UtcNow;
        AddDomainEvent(new TimeFrameActiveChangeEvent(Id, IsActive));
    }
}