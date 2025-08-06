namespace SportField.FieldService.Domain.Exceptions;

public class FieldNotFoundException : Exception
{
    public FieldNotFoundException(string fieldId) 
        : base($"Field with ID {fieldId} was not found.") { }
} 