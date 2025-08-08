namespace SportField.SharedKernel.DomainBase.Exceptions;

public class EntityNameAlreadyExistsException(string entity, string name)
    : Exception($"A {entity} named {name} already exists")
{
}
