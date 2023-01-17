using System;
using Newtonsoft.Json;

namespace SellAI.Models.AI.Objects
{
  public class ValuesDate
  {
    [JsonProperty("type")]
    public string Type { get; set; } = null!;

    [JsonProperty("grain")]
    public string? Grain { get; set; }

    [JsonProperty("value")]
    public string? Value { get; set; }

    [JsonProperty("from")]
    public FromTo? From { get; set; }

    [JsonProperty("to")]
    public FromTo? To { get; set; }
  }
}

