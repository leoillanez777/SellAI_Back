using System;
namespace SellAI.Interfaces
{
	public interface IAuthentication
	{
		Task<string> ValidAsync(string userName, string password);
	}
}

