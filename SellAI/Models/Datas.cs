using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using SellAI.Models.AI.Objects;

namespace SellAI.Models
{
  public class Datas
  {
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }

    [BsonElement("intent")]
    public Intents Intencion { get; set; } = null!;

    [BsonElement("entities")]
    public IDictionary<string, List<Entities>> Entidades { get; set; } = null!;

    [BsonElement("app")]
    public string? App { get; set; } = null!;
  }
}

