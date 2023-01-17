using System;
using Newtonsoft.Json;

namespace SellAI.Models.AI.Objects
{
  public class Entities
  {
    [JsonProperty("id")]
    public string Id { get; set; } = null!;

    [JsonProperty("name")]
    public string Name { get; set; } = null!;

    [JsonProperty("role")]
    public string Role { get; set; } = null!;

    [JsonProperty("type")]
    public string Type { get; set; } = null!;

    [JsonProperty("body")]
    public string Body { get; set; } = null!;

    [JsonProperty("value")]
    public string? Value { get; set; }

    [JsonProperty("confidence")]
    public decimal Confidence { get; set; }

    [JsonProperty("start")]
    public int Start { get; set; }

    [JsonProperty("end")]
    public int End { get; set; }

    [JsonProperty("suggested")]
    public bool? Suggested { get; set; }

    [JsonProperty("unit")]
    public string? Unit { get; set; }

    [JsonProperty("product")]
    public string? Product { get; set; }

    [JsonProperty("from")]
    public FromTo? From { get; set; }

    [JsonProperty("to")]
    public FromTo? To { get; set; }

    [JsonProperty("normalized")]
    public FromTo? Normalized { get; set; }

    [JsonProperty("second")]
    public decimal? Second { get; set; }

    [JsonProperty("domain")]
    public string? Domain { get; set; }

    [JsonProperty("resolved")]
    public List<Values>? Resolved { get; set; }

    [JsonProperty("values")]
    public List<ValuesDate>? Values { get; set; }

    [JsonProperty("entities")]
    public IDictionary<string, object>? Entity { get; set; }

  }
}

