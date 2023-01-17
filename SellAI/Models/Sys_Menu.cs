using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using SellAI.Models.Objects;

namespace SellAI.Models
{
  public class Sys_Menu
  {
    [BsonId]
    [BsonRepresentation (BsonType.ObjectId)]
    public string? Id { get; set; }

    [BsonElement ("name")]
    public string Nombre { get; set; } = null!;

    [BsonElement("collection")]
    public string Collection { get; set; } = null!;

    [BsonElement ("id_intent")]
    public string IntentID { get; set; } = null!;

    [BsonElement("entities")]
    public List<Entity>? Entities { get; set; }

    [BsonElement ("pdf")]
    public bool? Pdf { get; set; }

    [BsonElement("roles")]
    public List<Roles>? Roles { get; set; }

    [BsonElement ("app")]
    public string App { get; set; } = null!;

    [BsonExtraElements]
    public IDictionary<string, object>? Bucket { get; set; }
  }
}

