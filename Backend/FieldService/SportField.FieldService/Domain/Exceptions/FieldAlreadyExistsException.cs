namespace SportField.FieldService.Domain.Exceptions;

public class FieldAlreadyExistsException : Exception
{
    public FieldAlreadyExistsException(string fieldName) 
        : base($"Field with name {fieldName} already exists.") { }
} 