using System;
using Newtonsoft.Json;
using MongoDB.Bson.Serialization.Attributes;

namespace SellAI.Models.Objects
{
  public class EntityRol
  {
    [BsonElement("rolId")]
    [JsonProperty("rolId")]
    public string RolID { get; set; } = null!;

    [BsonElement("rol")]
    [JsonProperty("rol")]
    public string Rol { get; set; } = null!;

    [BsonElement("alias")]
    [JsonProperty("alias")]
    public string Alias { get; set; } = null!;
  }
}

