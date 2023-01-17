using System;
using System.Security.Cryptography;

namespace SellAI.Interfaces
{
	public interface IPassword
	{
		string GeneratePassword(string password);
		string GetPassword(string source, bool verifyPassword = false);
		string GetHash(HashAlgorithm hashAlgorithm, string input);
		bool VerifyHash(HashAlgorithm hashAlgorithm, string input, string hash);
	}
}

