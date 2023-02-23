using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace SellAI.Models.AI.Objects
{
  public class Intents
  {
    [BsonElement("id")]
    public string Id { get; set; } = null!;
    [BsonElement("name")]
    public string Name { get; set; } = null!;
    [BsonRepresentation(BsonType.Decimal128)]
    [BsonElement("confidence")]
    public decimal Confidence { get; set; }
  }
}

