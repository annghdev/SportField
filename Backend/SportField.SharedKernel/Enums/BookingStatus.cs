namespace SportField.SharedKernel.Enums
{
    public enum BookingStatus
    {
        Pending = 0,
        Expired = 1, // Quá hạn duyệt
        Confirmed = 2,
        Cancelled = 3,
        InProgress = 4,
        NoShow = 5, // Khách không đến
        Completed = 6, // Đã kết thúc
        Blocked = 7 // Nghi vấn, gian lận
    }
}
