namespace Common.Enums
{
    public enum BookingStatus
    {
        Pending = 0,
        Confirmed = 1,
        CancelledByUser = 2,
        CancelledByAdmin = 3,
        CancelledByDueToNonPayment = 4,
        PartiallyPaid = 5,
        Paid = 6,
        Expired = 7,
        NoShow = 8,
        Refunded = 9,
        Completed = 10,
        Blocked = 11
    }
}
