namespace SportField.IdentityService.Domain.Exceptions;

public class UserNotFoundException : Exception
{
    public UserNotFoundException(Guid userId) 
        : base($"User with ID '{userId}' was not found.")
    {
    }

    public UserNotFoundException(string identifier) 
        : base($"User with identifier '{identifier}' was not found.")
    {
    }

    public UserNotFoundException(string message, Exception innerException) 
        : base(message, innerException)
    {
    }
}
