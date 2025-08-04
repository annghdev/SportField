using System;

namespace SportField.FieldService.Domain.Exceptions;

public class InvalidFieldStatusException : Exception
{
    public InvalidFieldStatusException(string status) 
        : base($"Invalid status {status} for field.") { }
} 