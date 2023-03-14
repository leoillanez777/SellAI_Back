using System;
using SellAI.Models.DTOs.Object;

namespace SellAI.Models.DTOs
{
  public class MessageDTO
  {
    public List<string> messages { get; set; } = new();
    public Sys_ContextDTO contexts { get; set; } = null!;
    public TableDto table { get; set; } = new();
  }
}

