namespace Machine.Shopping.Api.Exceptions;

public class NotFoundException(string entity) : Exception($"{entity} não encontrado.");