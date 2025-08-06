using Common.Abstractions;
using Common.Enums;

namespace SportField.BookingService.Domain.Entities;

/// <summary>
/// Quản lý thanh toán cho booking với workflow phức tạp
/// </summary>
public class BookingPayment : AuditableEntity, IAggregateRoot
{
    public Guid BookingId { get; set; }
    public decimal Amount { get; set; }
    public PaymentMethod Method { get; set; }
    public PaymentStatus Status { get; set; } = PaymentStatus.Pending;
    
    // Timeout management
    public DateTime? DueDate { get; set; } // Hạn thanh toán
    public TimeSpan? TimeoutDuration { get; set; } // 15 minutes for gateway/transfer
    
    // Payment proof (for bank transfer)
    public string? ProofImageUrl { get; set; }
    public DateTime? ProofUploadedDate { get; set; }
    
    // Approval workflow
    public bool RequiresApproval { get; set; } = false;
    public string? ApprovedByAdminId { get; set; }
    public DateTime? ApprovedDate { get; set; }
    public string? ApprovalNotes { get; set; }
    
    // Transaction info
    public string? TransactionId { get; set; } // From payment gateway
    public string? GatewayResponse { get; set; }
    public string? GatewayTransactionId { get; set; }
    
    // Refund info
    public decimal? RefundedAmount { get; set; }
    public DateTime? RefundedDate { get; set; }
    public string? RefundReason { get; set; }
    public string? RefundTransactionId { get; set; }
    
    // Navigation properties
    public virtual Booking Booking { get; set; } = null!;
    public virtual ICollection<PaymentTransaction> Transactions { get; set; } = [];

    // Factory methods
    public static BookingPayment CreateGatewayPayment(Guid bookingId, decimal amount, PaymentMethod method)
    {
        TimeSpan? timeout = method == PaymentMethod.MoMo || method == PaymentMethod.VnPay 
            ? TimeSpan.FromMinutes(15) 
            : null;

        return new BookingPayment
        {
            Id = Guid.CreateVersion7(),
            BookingId = bookingId,
            Amount = amount,
            Method = method,
            TimeoutDuration = timeout,
            DueDate = timeout.HasValue ? DateTime.UtcNow.Add(timeout.Value) : null,
            CreatedDate = DateTime.UtcNow
        };
    }

    public static BookingPayment CreateBankTransferPayment(Guid bookingId, decimal amount)
    {
        var timeout = TimeSpan.FromMinutes(15);
        
        return new BookingPayment
        {
            Id = Guid.CreateVersion7(),
            BookingId = bookingId,
            Amount = amount,
            Method = PaymentMethod.BankTransfer,
            Status = PaymentStatus.WaitingProof,
            TimeoutDuration = timeout,
            DueDate = DateTime.UtcNow.Add(timeout),
            RequiresApproval = true,
            CreatedDate = DateTime.UtcNow
        };
    }

    public static BookingPayment CreatePayLaterPayment(Guid bookingId, decimal amount)
    {
        return new BookingPayment
        {
            Id = Guid.CreateVersion7(),
            BookingId = bookingId,
            Amount = amount,
            Method = PaymentMethod.PayLater,
            Status = PaymentStatus.WaitingApproval,
            RequiresApproval = true,
            CreatedDate = DateTime.UtcNow
        };
    }

    public static BookingPayment CreateAdminPayment(Guid bookingId, decimal amount)
    {
        return new BookingPayment
        {
            Id = Guid.CreateVersion7(),
            BookingId = bookingId,
            Amount = amount,
            Method = PaymentMethod.Cash,
            Status = PaymentStatus.Completed,
            CreatedDate = DateTime.UtcNow
        };
    }

    // Business methods
    public void SubmitProof(string proofImageUrl)
    {
        if (Method != PaymentMethod.BankTransfer)
            throw new InvalidOperationException("Proof is only required for bank transfer");

        ProofImageUrl = proofImageUrl;
        ProofUploadedDate = DateTime.UtcNow;
        Status = PaymentStatus.WaitingApproval;
        ModifiedDate = DateTime.UtcNow;
    }

    public void ApprovePayment(string adminId, string? notes = null)
    {
        if (!RequiresApproval)
            throw new InvalidOperationException("This payment does not require approval");

        Status = PaymentStatus.Completed;
        ApprovedByAdminId = adminId;
        ApprovedDate = DateTime.UtcNow;
        ApprovalNotes = notes;
        ModifiedDate = DateTime.UtcNow;
    }

    public void RejectPayment(string adminId, string reason)
    {
        Status = PaymentStatus.Failed;
        ApprovedByAdminId = adminId;
        ApprovedDate = DateTime.UtcNow;
        ApprovalNotes = $"Rejected: {reason}";
        ModifiedDate = DateTime.UtcNow;
    }

    public void CompleteGatewayPayment(string transactionId, string gatewayTransactionId, string? response = null)
    {
        Status = PaymentStatus.Completed;
        TransactionId = transactionId;
        GatewayTransactionId = gatewayTransactionId;
        GatewayResponse = response;
        ModifiedDate = DateTime.UtcNow;
    }

    public void FailGatewayPayment(string? response = null)
    {
        Status = PaymentStatus.Failed;
        GatewayResponse = response;
        ModifiedDate = DateTime.UtcNow;
    }

    public void ExpirePayment()
    {
        if (IsExpired())
        {
            Status = PaymentStatus.Expired;
            ModifiedDate = DateTime.UtcNow;
        }
    }

    public void ProcessRefund(decimal refundAmount, string reason, string? refundTransactionId = null)
    {
        if (Status != PaymentStatus.Completed)
            throw new InvalidOperationException("Can only refund completed payments");

        RefundedAmount = refundAmount;
        RefundedDate = DateTime.UtcNow;
        RefundReason = reason;
        RefundTransactionId = refundTransactionId;
        
        Status = refundAmount >= Amount ? PaymentStatus.Refunded : PaymentStatus.PartiallyRefunded;
        ModifiedDate = DateTime.UtcNow;
    }

    // Query methods
    public bool IsExpired()
    {
        return DueDate.HasValue && DateTime.UtcNow > DueDate.Value;
    }

    public TimeSpan? GetRemainingTime()
    {
        if (!DueDate.HasValue) return null;
        
        var remaining = DueDate.Value - DateTime.UtcNow;
        return remaining > TimeSpan.Zero ? remaining : TimeSpan.Zero;
    }

    public bool CanBeRefunded()
    {
        return Status == PaymentStatus.Completed && !RefundedDate.HasValue;
    }

    public bool RequiresProof()
    {
        return Method == PaymentMethod.BankTransfer && Status == PaymentStatus.WaitingProof;
    }

    public bool RequiresAdminAction()
    {
        return Status == PaymentStatus.WaitingApproval;
    }

    public decimal GetRefundableAmount()
    {
        if (!CanBeRefunded()) return 0;
        return Amount - (RefundedAmount ?? 0);
    }
}