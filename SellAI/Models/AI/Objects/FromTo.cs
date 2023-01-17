using System;
using Newtonsoft.Json;

namespace SellAI.Models.AI.Objects
{
  public class FromTo
  {
    [JsonProperty("unit")]
    public string? Unit { get; set; }

    [JsonProperty("grain")]
    public string? Grain { get; set; }

    [JsonProperty("product")]
    public string? Product { get; set; }

    [JsonProperty("value")]
    public decimal Value { get; set; }
  }
}

