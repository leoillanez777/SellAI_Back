using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace SellAI.Models
{
  public class Sys_Log
  {
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }

    [BsonElement("collection")]
    public string Coleccion { get; set; } = null!;

    [BsonElement("data")]
    public string Datos { get; set; } = null!;

    [BsonElement("command")]
    public string Comando { get; set; } = null!;

    [BsonElement("user")]
    public string Usuario { get; set; } = null!;

    [BsonElement("date")]
    public DateTime Fecha { get; set; } = DateTime.UtcNow;

    [BsonElement("app")]
    public string App { get; set; } = null!;
  }
}

