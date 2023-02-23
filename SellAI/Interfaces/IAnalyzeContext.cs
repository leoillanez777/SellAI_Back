using System;
using SellAI.Models.AI;
using SellAI.Models.DTOs;

namespace SellAI.Interfaces
{
  public interface IAnalyzeContext
  {

    /// <summary>
    /// Get message of sys_context and sys_menu
    /// </summary>
    /// <param name="response">response of wit.ai</param>
    /// <param name="roleApp">data of claims</param>
    /// <param name="previousIntentID">only if it has previous intent.</param>
    /// <param name="previousContextID">only if it has previous context.</param>
    /// <returns>messages for user</returns>
    Task<MessageDTO> GetMessagesAsync(Message response, RoleAppDTO roleApp, string previousIntentID = "", string previousContextID = "");
  }
}

