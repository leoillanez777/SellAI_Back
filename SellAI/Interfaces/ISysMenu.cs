using System;
using SellAI.Models;
using SellAI.Models.AI;
using SellAI.Models.DTOs;

namespace SellAI.Interfaces
{
  public interface ISysMenu
  {
    /// <summary>
    /// Get intent of Sys_Menu on MongoDB
    /// </summary>
    /// <param name="intentName">name of intent to find</param>
    /// <param name="roleApp">class with claims of security</param>
    /// <returns>data of mongoDB or null</returns>
    Task<Sys_Menu> GetIntentAsync(string intentName, RoleAppDTO roleApp);

    /// <summary>
    /// Get of mongoDB if it is greeting.
    /// </summary>
    /// <param name="nameIntent">name of intent.</param>
    /// <returns>return data of sys_menu of greeting or null.</returns>
    Task<Sys_Menu> GetGreetingAsync(string nameIntent);

    /// <summary>
    /// Get message of mongoDB for out of scope.
    /// </summary>
    /// <returns>Message</returns>
    Task<string> GetOutOfScopeAsync();
  }
}

