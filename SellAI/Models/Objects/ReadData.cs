using System;
namespace SellAI.Models.Objects
{
  public class ReadData
  {
    public string Command { get; set; } = null!;
    public string FullPath { get; set; } = null!;
    public string? ExtraCmd { get; set; }
    public string? CondExtra { get; set; }
    public string Value { get; set; } = null!;
  }
}

