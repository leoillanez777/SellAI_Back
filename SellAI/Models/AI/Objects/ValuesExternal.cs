using System;
using Newtonsoft.Json;

namespace SellAI.Models.AI.Objects
{
  public class ValuesExternal
  {
    [JsonProperty("geonames")]
    public string? GeoNames { get; set; }

    [JsonProperty("wikidata")]
    public string Wikidata { get; set; } = null!;

    [JsonProperty("wikipedia")]
    public string Wikipedia { get; set; } = null!;
  }
}

