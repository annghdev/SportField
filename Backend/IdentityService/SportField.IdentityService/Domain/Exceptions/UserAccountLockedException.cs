namespace SportField.IdentityService.Domain.Exceptions;

public class UserAccountLockedException : Exception
{
    public DateTime LockoutEndDate { get; }
    public int FailedAttempts { get; }

    public UserAccountLockedException(DateTime lockoutEndDate, int failedAttempts) 
        : base($"User account is locked until {lockoutEndDate:yyyy-MM-dd HH:mm:ss} UTC after {failedAttempts} failed attempts.")
    {
        LockoutEndDate = lockoutEndDate;
        FailedAttempts = failedAttempts;
    }

    public UserAccountLockedException(string message, DateTime lockoutEndDate, int failedAttempts) 
        : base(message)
    {
        LockoutEndDate = lockoutEndDate;
        FailedAttempts = failedAttempts;
    }
}
