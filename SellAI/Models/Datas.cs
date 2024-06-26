﻿using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;
using SellAI.Models.AI.Objects;

namespace SellAI.Models
{
  public class Datas
  {
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    [JsonProperty("_id")]
    public string? Id { get; set; }

    [BsonElement("intent")]
    [JsonProperty("intent")]
    public Intents Intencion { get; set; } = null!;

    [BsonElement("entities")]
    [JsonProperty("entities")]
    public IDictionary<string, List<Entities>> Entidades { get; set; } = null!;

    [BsonElement("app")]
    [JsonProperty("app")]
    public string? App { get; set; } = null!;
  }
}

