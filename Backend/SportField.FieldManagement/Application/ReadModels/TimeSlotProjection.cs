namespace SportField.FieldManagement.Application.ReadModels;

public class TimeSlotProjection
{
    public Guid Id { get; set; }
    public TimeOnly StartTime { get; set; }
    public TimeOnly EndTime { get; set; }
    public int Numering { get; set; } // số thứ tự để dễ validate khung giờ liên tục
    public int DurationInMinutes { get; set; }
    public string Display { get; set; } = string.Empty;
    public bool IsActive { get; set; }
}
