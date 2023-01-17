using System;
using Microsoft.AspNetCore.Mvc;
using SellAI.Models.AI;

namespace SellAI.Interfaces
{
	public interface IInterpreter
	{
		Task<string> SendMessageAsync(string message);
    Task<string> SendResponseAsync(string message, string token);
  }
}

