namespace Machine.Shopping.Api.Exceptions;

public class AlreadyExistsException(string entity) : Exception($"{entity} já está cadastrado.");