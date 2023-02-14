using System;
namespace SellAI.Models.DTOs
{
  public class MessageDTO
  {
    public List<string> messages { get; set; } = null!;
    public Sys_ContextDTO contexts { get; set; } = null!;
  }
}

