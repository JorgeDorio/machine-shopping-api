﻿using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Machine.Shopping.Api.Models.User;

public class User
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }

    public required string Email { get; set; }
    public required string? TenantId { get; set; }
    public required string PasswordHash { get; set; }
    public required string Name { get; set; }
}