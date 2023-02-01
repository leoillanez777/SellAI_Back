using System;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using SellAI.Models.AI;
using SellAI.Models.DTOs;

namespace SellAI.Interfaces
{
	public interface IInterpreter
	{
		Task<string> SendMessageAsync(string message, RoleAppDTO roleApp);
    Task<string> SendResponseAsync(string message, string token, string appName);
  }
}

