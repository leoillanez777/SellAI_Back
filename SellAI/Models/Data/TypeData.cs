using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;

namespace SellAI.Models.Data
{
  public class TypeData
  {
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    [JsonProperty("_id")]
    public string? Id { get; set; }

    [BsonElement("name")]
    [JsonProperty("name")]
    public string Nombre { get; set; } = null!;

    [BsonElement("description")]
    [JsonProperty("description")]
    public string Descripcion { get; set; } = null!;

    [BsonElement("synonyms")]
    [JsonProperty("synonyms")]
    public List<string> Sinonimos { get; set; } = null!;


    [BsonElement("isActive")]
    [JsonProperty("isActive")]
    public bool Activo { get; set; }

    [BsonElement("app")]
    [JsonProperty("app")]
    public string App { get; set; } = null!;

  }
}

