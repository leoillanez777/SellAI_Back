using System;
using Newtonsoft.Json;
using SellAI.Models.AI.Objects;

namespace SellAI.Models.AI
{
  public class Audio
  {
    [JsonProperty("text")]
    public string Text { get; set; } = null!;

    [JsonProperty("intents")]
    public List<Intents>? Intents { get; set; }

    [JsonProperty("entities")]
    public IDictionary<string, List<Entities>>? Entities { get; set; }

    [JsonProperty("is_final")]
    public bool? EsFinal { get; set; }
  }
}

