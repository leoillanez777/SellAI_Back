using System;
using MongoDB.Bson.Serialization.Attributes;

namespace SellAI.Models.Objects
{
  public class Roles
  {
    [BsonElement("rol")]
    public string Rol { get; set; } = null!;
    [BsonElement("intent")]
    public string Intent { get; set; } = null!;
  }
}

