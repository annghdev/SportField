namespace SportField.IdentityService.Domain.Exceptions;

public class DuplicateUserException : Exception
{
    public string ConflictField { get; }

    public DuplicateUserException(string conflictField, string value) 
        : base($"A user with {conflictField} '{value}' already exists.")
    {
        ConflictField = conflictField;
    }

    public DuplicateUserException(string message, string conflictField, Exception innerException) 
        : base(message, innerException)
    {
        ConflictField = conflictField;
    }
}
