using System;
using SellAI.Models;
using System.Security.Claims;
using SellAI.Models.DTOs;

namespace SellAI.Interfaces
{
  public interface ISysContext
  {
    /// <summary>
    /// Get data of sys_context for id
    /// </summary>
    /// <param name="Id">id</param>
    /// <returns></returns>
    Task<Sys_Context> GetContextAsync(string Id);

    /// <summary>
    /// Create sys_context
    /// </summary>
    /// <param name="sys_Context">class of context</param>
    /// <returns>class with id create</returns>
    Task<Sys_Context> CreateContextAsync(Sys_Context sys_Context);

    /// <summary>
    /// Update sys_context
    /// </summary>
    /// <param name="sys_Context">class of context</param>
    /// <returns>class with data updated</returns>
    Task<Sys_Context> UpdateContextAsync(Sys_Context sys_Context);
  }
}

