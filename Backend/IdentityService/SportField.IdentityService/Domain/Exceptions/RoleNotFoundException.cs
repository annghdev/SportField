namespace SportField.IdentityService.Domain.Exceptions;

public class RoleNotFoundException : Exception
{
    public RoleNotFoundException(Guid roleId) 
        : base($"Role with ID '{roleId}' was not found.")
    {
    }

    public RoleNotFoundException(string roleName) 
        : base($"Role with name '{roleName}' was not found.")
    {
    }

    public RoleNotFoundException(string message, Exception innerException) 
        : base(message, innerException)
    {
    }
}
