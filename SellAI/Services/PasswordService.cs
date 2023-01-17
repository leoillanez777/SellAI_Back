using System;
using System.Security.Cryptography;
using System.Text;
using SellAI.Interfaces;

namespace SellAI.Services
{
	public class PasswordService : IPassword
	{
		public PasswordService()
		{
		}

    /// <summary>
    /// Recover or verfiy password is ok.
    /// </summary>
    /// <param name="source">Unencrypted password</param>
    /// <param name="verifyPassword">indicates whether to verify or return hash</param>
    /// <returns>Ok (verifyPassword) or Hash(Password) or Error</returns>
    /// <exception cref="NotImplementedException"></exception>
    public string GetPassword (string source, bool verifyPassword = false)
    {
      using (SHA256 sha256Hash = SHA256.Create())
      {
        string hash = GetHash(sha256Hash, source);

        if (VerifyHash(sha256Hash, source, hash))
        {
          if (verifyPassword)
            return "Ok";
          else
            return hash;
        }
        else
          return "Error";
      }
    }

    /// <summary>
    /// Generate hash for password
    /// </summary>
    /// <param name="password">Unencrypted password</param>
    /// <returns>Encrypted Password</returns>
    /// <exception cref="NotImplementedException"></exception>
    public string GeneratePassword (string password)
		{
      using (SHA256 sha256Hash = SHA256.Create())
      {
        string hash = GetHash(sha256Hash, password);
        if (VerifyHash(sha256Hash, password, hash))
        {
          return hash;
        }
        else
        {
          throw new Exception("Error al generar hash");
        }
      }
    }

    /// <summary>
    /// Generated new Hash
    /// </summary>
    /// <param name="hashAlgorithm">Hash Type</param>
    /// <param name="input">String convert to hash</param>
    /// <returns>Input in format hash</returns>
		public string GetHash(HashAlgorithm hashAlgorithm, string input)
    {
      byte[] data = hashAlgorithm.ComputeHash(Encoding.UTF8.GetBytes(input));
      var sBuilder = new StringBuilder();
      for (int i = 0; i < data.Length; i++)
      {
        sBuilder.Append(data[i].ToString("x2"));
      }
      return sBuilder.ToString();
    }

    /// <summary>
    /// Compare input string with hash parameter
    /// </summary>
    /// <param name="hashAlgorithm">Hash Type</param>
    /// <param name="input">string to compare</param>
    /// <param name="hash">string hash compare with input</param>
    /// <returns>True it's equals or False it's not</returns>
		public bool VerifyHash (HashAlgorithm hashAlgorithm, string input, string hash)
		{
			var hashOfInput = GetHash (hashAlgorithm, input);
			StringComparer comparer = StringComparer.OrdinalIgnoreCase;
			return comparer.Compare (hashOfInput, hash) == 0;
		}
	}
}

