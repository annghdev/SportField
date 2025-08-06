using Common.Abstractions;
using Common.Enums;

namespace SportField.BookingService.Domain.Entities;

/// <summary>
/// Chi tiết các giao dịch thanh toán (có thể có nhiều attempts)
/// </summary>
public class PaymentTransaction : BaseEntity
{
    public Guid BookingPaymentId { get; set; }
    public decimal Amount { get; set; }
    public PaymentMethod Method { get; set; }
    public PaymentStatus Status { get; set; }
    
    // Transaction details
    public string? ExternalTransactionId { get; set; }
    public string? GatewayReference { get; set; }
    public string? GatewayResponse { get; set; }
    public DateTime? ProcessedDate { get; set; }
    
    // Retry/attempt info
    public int AttemptNumber { get; set; } = 1;
    public bool IsRetry { get; set; } = false;
    public string? FailureReason { get; set; }
    
    // Navigation properties
    public virtual BookingPayment BookingPayment { get; set; } = null!;

    public static PaymentTransaction Create(Guid bookingPaymentId, decimal amount, PaymentMethod method, int attemptNumber = 1)
    {
        return new PaymentTransaction
        {
            Id = Guid.CreateVersion7(),
            BookingPaymentId = bookingPaymentId,
            Amount = amount,
            Method = method,
            Status = PaymentStatus.Processing,
            AttemptNumber = attemptNumber,
            IsRetry = attemptNumber > 1,
        };
    }

    public void MarkAsCompleted(string externalTransactionId, string? gatewayReference = null, string? response = null)
    {
        Status = PaymentStatus.Completed;
        ExternalTransactionId = externalTransactionId;
        GatewayReference = gatewayReference;
        GatewayResponse = response;
        ProcessedDate = DateTime.UtcNow;
    }

    public void MarkAsFailed(string reason, string? response = null)
    {
        Status = PaymentStatus.Failed;
        FailureReason = reason;
        GatewayResponse = response;
        ProcessedDate = DateTime.UtcNow;
    }

    public bool IsSuccessful()
    {
        return Status == PaymentStatus.Completed;
    }

    public bool CanRetry()
    {
        return Status == PaymentStatus.Failed && AttemptNumber < 3; // Max 3 attempts
    }
}