using System;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using SellAI.Models.AI;

namespace SellAI.Interfaces
{
	public interface IInterpreter
	{
		Task<string> SendMessageAsync(string message, ClaimsIdentity identity, string id);
    Task<string> SendResponseAsync(string message, string token);
  }
}

