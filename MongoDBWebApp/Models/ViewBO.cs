using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Text.Json.Serialization;

namespace MongoDBWebApp.Models;

public class ViewBO
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    [BsonElement("_id")]
    [JsonPropertyName("_id")]
    public string? Id { get; set; }

    [BsonElement("entityName")]
    [JsonPropertyName("entityName")]
    public string EntityName { get; set; } = null!;

    [BsonElement("fields")]
    [JsonPropertyName("fields")]
    public List<FieldBO>? Fields { get; set; }

}