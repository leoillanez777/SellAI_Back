using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using SellAI.Models.Objects;

namespace SellAI.Models
{
  public class User
  {
    [BsonId]
    [BsonRepresentation (BsonType.ObjectId)]
    public string? Id { get; set; }

    [BsonElement ("name")]
    public string Nombre { get; set; } = null!;

    [BsonElement ("email")]
    public string Email { get; set; } = null!;

    [BsonElement ("phone")]
    public string Celular { get; set; } = null!;

    [BsonElement ("password")]
    public string Password { get; set; } = null!;

    [BsonElement ("code")]
    public string? Codigo { get; set; }

    [BsonElement ("user")]
    public string Usuario { get; set; } = null!;

    [BsonElement ("url")]
    public UrlDB? Urls { get; set; }

    [BsonElement ("deposit")]
    public string? Deposito { get; set; }

    [BsonElement ("createdAt")]
    public DateTime Creado { get; set; }

    [BsonElement ("lastAccess")]
    public DateTime UltimoAcceso { get; set; }

    [BsonElement ("isActive")]
    public bool Activo { get; set; }

    [BsonElement ("blocked")]
    public bool Bloqueado { get; set; }

    [BsonElement ("isLockedOut")]
    public bool IsLockedOut { get; set; }

    [BsonElement ("token")]
    [BsonIgnoreIfNull]
    public string Token { get; set; } = null!;

    [BsonElement ("app")]
    public string App { get; set; } = null!;

    [BsonExtraElements]
    public IDictionary<string, object>? Bucket { get; set; }
  }
}

