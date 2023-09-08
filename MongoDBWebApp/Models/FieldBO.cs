using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Text.Json.Serialization;

namespace MongoDBWebApp.Models;

public class FieldBO
{
    [BsonElement("displayName")]
    [JsonPropertyName("displayName")]
    public string DisplayName { get; set; } = null!;

    [BsonElement("attrName")]
    [JsonPropertyName("attrName")]
    public string? AttrName { get; set; } = null!;

    [BsonElement("type")]
    [JsonPropertyName("type")]
    public string? Type { get; set; } = null!;

}