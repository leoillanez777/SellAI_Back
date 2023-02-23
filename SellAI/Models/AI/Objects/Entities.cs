using System;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;

namespace SellAI.Models.AI.Objects
{
  public class Entities
  {
    [JsonProperty("id")]
    [BsonElement("id")]
    public string Id { get; set; } = null!;

    [JsonProperty("name")]
    [BsonElement("name")]
    public string Name { get; set; } = null!;

    [JsonProperty("role")]
    [BsonElement("role")]
    public string Role { get; set; } = null!;

    [JsonProperty("type")]
    [BsonElement("type")]
    public string Type { get; set; } = null!;

    [JsonProperty("body")]
    [BsonElement("body")]
    public string Body { get; set; } = null!;

    [JsonProperty("value")]
    [BsonElement("value")]
    public string? Value { get; set; }

    [JsonProperty("confidence")]
    [BsonElement("confidence")]
    public decimal Confidence { get; set; }

    [JsonProperty("start")]
    [BsonElement("start")]
    public int Start { get; set; }

    [JsonProperty("end")]
    [BsonElement("end")]
    public int End { get; set; }

    [JsonProperty("suggested")]
    [BsonElement("suggested")]
    public bool? Suggested { get; set; }

    [JsonProperty("unit")]
    [BsonElement("unit")]
    public string? Unit { get; set; }

    [JsonProperty("product")]
    [BsonElement("product")]
    public string? Product { get; set; }

    [JsonProperty("from")]
    [BsonElement("from")]
    public FromTo? From { get; set; }

    [JsonProperty("to")]
    [BsonElement("to")]
    public FromTo? To { get; set; }

    [JsonProperty("normalized")]
    [BsonElement("normalized")]
    public FromTo? Normalized { get; set; }

    [JsonProperty("second")]
    [BsonElement("second")]
    public decimal? Second { get; set; }

    [JsonProperty("domain")]
    [BsonElement("domain")]
    public string? Domain { get; set; }

    [JsonProperty("resolved")]
    [BsonElement("resolved")]
    public List<Values>? Resolved { get; set; }

    [JsonProperty("values")]
    [BsonElement("values")]
    public List<ValuesDate>? Values { get; set; }

    [JsonProperty("entities")]
    [BsonElement("entities")]
    public IDictionary<string, object>? Entity { get; set; }

  }
}

