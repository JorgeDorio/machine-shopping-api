using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Machine.Shopping.Api.Models.Customer;

public class Customer
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }
    public required string Phone { get; set; }
    public required string Cpf { get; set; }
    public required string TenantId { get; set; }
    public required string Name { get; set; }
}