using System;
using MongoDB.Bson.Serialization.Attributes;

namespace SellAI.Models.Objects
{
  public class UrlDB
  {
    [BsonElement ("type")]
    public string Type { get; set; } = null!;

    [BsonElement("url")]
    public string? URL { get; set; }
  }
}

