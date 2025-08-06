namespace SportField.FieldService.Domain.Exceptions;

public class InvalidFieldNameException : Exception
{
    public InvalidFieldNameException(string message) 
        : base(message) { }
} 