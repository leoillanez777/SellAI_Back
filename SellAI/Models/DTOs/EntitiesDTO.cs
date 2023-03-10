using System;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;

namespace SellAI.Models.DTOs
{
  public class EntitiesDTO
  {
    [JsonProperty("name")]
    [BsonElement("name")]
    public string Name { get; set; } = null!;

    [JsonProperty("role")]
    [BsonElement("role")]
    public string Role { get; set; } = null!;

    [JsonProperty("type")]
    [BsonElement("type")]
    public string Type { get; set; } = null!;

    [JsonProperty("body")]
    [BsonElement("body")]
    public string Body { get; set; } = null!;

    [JsonProperty("display")]
    [BsonElement("display")]
    public string? Display { get; set; }
  }
}

