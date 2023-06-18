using System;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;

namespace SellAI.Models.Objects;

public class Entity {
  [BsonElement("name")]
  [JsonProperty("name")]
  public string Name { get; set; } = null!;

  [BsonElement("alias")]
  [JsonProperty("alias")]
  public string Alias { get; set; } = null!;

  [BsonElement("roleId")]
  [JsonProperty("roleId")]
  public string? RoleId { get; set; }

  [BsonElement("role")]
  [JsonProperty("role")]
  public string? Role { get; set; }

  [BsonElement("required")]
  [JsonProperty("required")]
  public bool? Required { get; set; }

  [BsonElement("value")]
  [JsonProperty("value")]
  public string? Value { get; set; }

  [BsonElement("search")]
  [JsonProperty("search")]
  public string? Search { get; set; }

  [BsonElement("message")]
  [JsonProperty("message")]
  public string? Message { get; set; }
}
