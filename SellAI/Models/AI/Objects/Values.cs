using System;
using Newtonsoft.Json;

namespace SellAI.Models.AI.Objects
{
  public class Values
  {
    [JsonProperty("name")]
    public string Name { get; set; } = null!;

    [JsonProperty("domain")]
    public string Domain { get; set; } = null!;

    [JsonProperty("external")]
    public ValuesExternal? External { get; set; }

    [JsonProperty("timezone")]
    public string? Timezone { get; set; }

    [JsonProperty("coords")]
    public Coords? Coords { get; set; }
  }
}

