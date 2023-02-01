using System;
using SellAI.Models;
using System.Security.Claims;
using SellAI.Models.DTOs;

namespace SellAI.Interfaces
{
  public interface ISysContext
  {
    Task<Sys_Context> GetContextAsync(string Id);
    Task<Sys_Context> CreateContextAsync(Sys_Context sys_Context);
  }
}

