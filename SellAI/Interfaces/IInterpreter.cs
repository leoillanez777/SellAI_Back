using System;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using SellAI.Models.AI;
using SellAI.Models.DTOs;

namespace SellAI.Interfaces
{
	public interface IInterpreter
	{
		/// <summary>
		/// Use for messages without context.
		/// </summary>
		/// <param name="message">text send to the user</param>
		/// <param name="roleApp">claims</param>
		/// <returns>response of wit.ai</returns>
		Task<string> SendMessageAsync(string message, RoleAppDTO roleApp);

    /// <summary>
    /// Use for messages with context.
    /// </summary>
    /// <param name="message">text send to the user</param>
    /// <param name="token">id of context</param>
    /// <param name="roleApp">claims</param>
    /// <returns>response of wit.ai</returns>
    Task<string> SendResponseAsync(string message, string token, RoleAppDTO roleApp);
  }
}

