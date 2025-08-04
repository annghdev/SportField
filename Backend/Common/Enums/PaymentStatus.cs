namespace Common.Enums;

public enum PaymentStatus
{
    Pending = 0,        // Chờ thanh toán
    Processing = 1,     // Đang xử lý
    Completed = 2,      // Đã thanh toán
    Failed = 3,         // Thất bại
    Cancelled = 4,      // Đã hủy
    CancelledByOverDue = 5, // Đã hủy
    Refunded = 6        // Đã hoàn tiền
} 