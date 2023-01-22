using System;
using SellAI.Models.DTOs;

namespace SellAI.Interfaces
{
	public interface IAuthentication
	{
		Task<string> ValidAsync(string userName, string password);
		Task<SignInDTO> LoginAsync(LoginDTO login);
  }
}

