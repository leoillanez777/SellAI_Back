using System;
using MongoDB.Bson.Serialization.Attributes;

namespace SellAI.Models.Objects
{
  public class Entity
  {
    [BsonElement("id")]
    public string Id { get; set; } = null!;
    [BsonElement("name")]
    public string Name { get; set; } = null!;
    [BsonElement("alias")]
    public string Alias { get; set; } = null!;
    [BsonElement("required")]
    public bool? Required { get; set; }
  }
}

