namespace Common.Enums
{
    public enum BookingStatus
    {
        Pending = 0,
        Expired = 1, // Quá hạn duyệt
        Confirmed = 2,
        CancelledByUser = 3,
        CancelledByDueToNonPayment = 4,
        CancelledByAdmin = 5,
        NoShow = 6, // Khách không đến
        Completed = 7, // Đã kết thúc
        Blocked = 8 // Nghi vấn, gian lận
    }
}
