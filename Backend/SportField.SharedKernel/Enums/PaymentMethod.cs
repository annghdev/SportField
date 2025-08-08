namespace SportField.SharedKernel.Enums;

public enum PaymentMethod
{
    Cash = 1,           // Tiền mặt (admin booking)
    BankTransfer = 2,   // Chuyển khoản (cần ảnh minh chứng)
    MoMo = 3,          // MoMo e-wallet
    VnPay = 4,         // VnPay gateway
    PayLater = 5        // Trả sau (cần admin approve)
}