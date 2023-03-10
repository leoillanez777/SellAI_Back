using System;
using SellAI.Models.Objects;

namespace SellAI.Models.Data
{
  public class ResponseAnalyze
  {
    public bool AllFieldsComplete { get; set; }
    public List<string> Messages { get; set; } = new();
    public List<ReadData>? ReadDatas { get; set; }
  }
}

