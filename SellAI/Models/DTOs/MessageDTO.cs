using System;
namespace SellAI.Models.DTOs
{
  public class MessageDTO
  {
    public List<MsgDTO> msgDTOs { get; set; } = null!;
    public List<Sys_ContextDTO> contexts { get; set; } = null!;
  }
}

