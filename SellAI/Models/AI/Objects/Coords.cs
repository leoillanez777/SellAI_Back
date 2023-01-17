using System;
using Newtonsoft.Json;

namespace SellAI.Models.AI.Objects
{
  public class Coords
  {
    [JsonProperty("lat")]
    public decimal Latitud { get; set; }

    [JsonProperty("long")]
    public decimal Longitud { get; set; }
  }
}

