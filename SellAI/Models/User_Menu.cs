﻿using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;
using SellAI.Models.Objects;

namespace SellAI.Models
{
  public class User_Menu
  {
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    [JsonProperty("_id")]
    public string? Id { get; set; }

    [BsonElement("label")]
    [JsonProperty("label")]
    public string Label { get; set; } = null!;

    [BsonElement("icon")]
    [JsonProperty("icon")]
    public string? Icono { get; set; }

    [BsonElement("isActive")]
    [JsonProperty("isActive")]
    public bool EsActivo { get; set; }

    [BsonElement("roles")]
    [JsonProperty("roles")]
    public List<string> Roles { get; set; } = null!;

    [BsonElement("items")]
    [JsonProperty("items")]
    public List<ItemMenu> Items { get; set; } = null!;
  }
}

