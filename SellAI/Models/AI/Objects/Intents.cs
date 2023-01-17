using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace SellAI.Models.AI.Objects
{
  public class Intents
  {
    public string Id { get; set; } = null!;
    public string Name { get; set; } = null!;
    [BsonRepresentation(BsonType.Decimal128)]
    public decimal Confidence { get; set; }
  }
}

