using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;
using SellAI.Models.Objects;

namespace SellAI.Models
{
  public class Sys_Menu
  {
    [BsonId]
    [BsonRepresentation (BsonType.ObjectId)]
    [JsonProperty("id")]
    public string? Id { get; set; }

    [BsonElement("id_intent")]
    [JsonProperty("id_intent")]
    public string IntentID { get; set; } = null!;

    [BsonElement ("name")]
    [JsonProperty("name")]
    public string Nombre { get; set; } = null!;

    [BsonElement("display")]
    [JsonProperty("display")]
    public string Display { get; set; } = null!;

    [BsonElement("action")]
    [JsonProperty("action")]
    public string Accion { get; set; } = null!;

    [BsonElement("collection")]
    [JsonProperty("collection")]
    public string Collection { get; set; } = null!;

    [BsonElement("entities")]
    [JsonProperty("entities")]
    public List<Entity>? Entities { get; set; }

    [BsonElement ("pdf")]
    [JsonProperty("pdf")]
    public bool? Pdf { get; set; }

    [BsonElement("roles")]
    [JsonProperty("roles")]
    public List<string>? Roles { get; set; }

    [BsonElement ("app")]
    [JsonProperty("app")]
    public string App { get; set; } = null!;

    [BsonElement("message")]
    [JsonProperty("message")]
    public string? Mensaje { get; set; }

    [BsonExtraElements]
    public IDictionary<string, object>? Bucket { get; set; }
  }
}

