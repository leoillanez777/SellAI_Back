using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;
using SellAI.Models.AI;

namespace SellAI.Models
{
  public class Sys_Context
  {
    [BsonId]
    [BsonRepresentation (BsonType.ObjectId)]
    [JsonProperty("id")]
    public string? Id { get; set; }

    [BsonElement("display")]
    [JsonProperty("display")]
    public string Display { get; set; } = null!;

    [BsonElement("action")]
    [JsonProperty("action")]
    public string Accion { get; set; } = null!;

    [BsonElement("intents")]
    [JsonProperty("intents")]
    public Message Intents { get; set; } = null!;

    [BsonElement ("app")]
    [JsonProperty("app")]
    public string App { get; set; } = null!;

    [BsonExtraElements]
    public IDictionary<string, object>? Bucket { get; set; }
  }
}

