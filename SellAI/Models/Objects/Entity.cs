using System;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;

namespace SellAI.Models.Objects
{
  public class Entity
  {
    [BsonElement("entityID")]
    [JsonProperty("entityID")]
    public string EntityID { get; set; } = null!;

    [BsonElement("name")]
    [JsonProperty("name")]
    public string Name { get; set; } = null!;

    [BsonElement("alias")]
    [JsonProperty("alias")]
    public string Alias { get; set; } = null!;

    [BsonElement("required")]
    [JsonProperty("required")]
    public bool? Required { get; set; }

    [BsonElement("value")]
    [JsonProperty("value")]
    public string? Value { get; set; }

    [BsonElement("message")]
    [JsonProperty("message")]
    public string? Message { get; set; }

    [BsonElement("rol")]
    [JsonProperty("rol")]
    public string? Rol { get; set; }
  }
}

