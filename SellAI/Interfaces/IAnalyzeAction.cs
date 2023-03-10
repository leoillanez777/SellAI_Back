using System;
using SellAI.Models;
using SellAI.Models.Data;
using SellAI.Models.DTOs;
using SellAI.Models.Objects;

namespace SellAI.Interfaces
{
  public interface IAnalyzeAction
  {
    ResponseAnalyze ActionAndEntity(Sys_Menu sys_menu, List<Entity> listEntity);
  }
}

