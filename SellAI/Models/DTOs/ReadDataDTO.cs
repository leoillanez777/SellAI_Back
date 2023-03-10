using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;
using SellAI.Models.AI.Objects;

namespace SellAI.Models.DTOs
{
  public class ReadDataDTO
  {
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    [JsonProperty("_id")]
    public string? Id { get; set; }

    [BsonElement("entities")]
    [JsonProperty("entities")]
    public IDictionary<string, List<EntitiesDTO>> Entidades { get; set; } = null!;

  }
}

