using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Machine.Shopping.Api.Models.Customer.Tenant;

public class Tenant
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }

    public required string Alias { get; set; }
    public required string Name { get; set; }
}