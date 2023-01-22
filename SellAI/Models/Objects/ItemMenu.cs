using System;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;

namespace SellAI.Models.Objects
{
  [JsonObject]
  public class ItemMenu
  {
    [BsonElement("label")]
    [JsonProperty("label")]
    public string Label { get; set; } = null!;

    [BsonElement("icon")]
    [JsonProperty("icon")]
    public string? Icono { get; set; }

    [BsonElement("to")]
    [JsonProperty("to")]
    public string To { get; set; } = null!;

    [BsonElement("items")]
    [JsonProperty("items")]
    public List<ItemMenu>? Items { get; set; }

    [BsonElement("roles")]
    [JsonProperty("roles")]
    public List<string> Roles { get; set; } = null!;
  }
}

