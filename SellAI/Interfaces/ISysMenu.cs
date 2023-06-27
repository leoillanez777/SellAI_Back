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
    /// Get intent of sys_menu on MongoDB by intentID
    /// </summary>
    /// <param name="intentId">Id to search</param>
    /// <param name="roleApp">class with claims of security</param>
    /// <returns>data of mongo or null data.</returns>
    Task<Sys_Menu?> GetAsync(string intentId, RoleAppDTO roleApp);

    /// <summary>
    /// Get of mongoDB if it is greeting.
    /// </summary>
    /// <param name="nameIntent">name of intent.</param>
    /// <returns>return data of sys_menu of greeting or null.</returns>
    Task<Sys_Menu> GetGreetingAsync(string nameIntent);


    Task<string> PostAsync(Sys_MenuDTO sysMenuDTO, RoleAppDTO claims);

    /// <summary>
    /// Get message of mongoDB for out of scope.
    /// </summary>
    /// <returns>Message</returns>
    Task<string> GetOutOfScopeAsync();
  }
}

