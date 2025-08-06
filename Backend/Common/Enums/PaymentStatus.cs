namespace Common.Enums;

public enum PaymentStatus
{
    Pending = 1,            // Chờ thanh toán
    Processing = 2,         // Đang xử lý (gateway)
    WaitingProof = 3,       // Chờ minh chứng (bank transfer)
    WaitingApproval = 4,    // Chờ admin duyệt
    Completed = 5,          // Đã thanh toán thành công
    Failed = 6,             // Thất bại
    Cancelled = 7,          // Đã hủy
    Expired = 8,            // Hết hạn timeout
    Refunded = 9,           // Đã hoàn tiền
    PartiallyRefunded = 10  // Hoàn một phần
}