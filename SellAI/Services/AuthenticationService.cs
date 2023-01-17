using System;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using SellAI.Interfaces;
using MongoDB.Driver;
using SellAI.Models;
using Microsoft.Extensions.Options;

namespace SellAI.Services
{
	public class AuthenticationService : IAuthentication
	{
		private readonly string _secretKey = "";
		private readonly string _issuer = "";
		private readonly string _audience = "";
    private readonly IPassword _password;
    private readonly IMongoClient _client;
    private readonly IMongoCollection<User> _db;

		public AuthenticationService(IMongoClient client, IOptions<ContextMongoDB> options, IConfiguration configuration, IPassword password)
		{
      _client = client;
      _db = _client.GetDatabase(options.Value.DatabaseName).GetCollection<User>(options.Value.UserCollectionName);
      _password = password;
      // Devuelve configuracion del token.
      IConfigurationSection secJwt = configuration.GetSection("JWT")!;
      _secretKey = secJwt.GetSection("Key").ToString()!;
      _issuer = secJwt.GetSection("Issuer").ToString()!;
      _audience = secJwt.GetSection("Audience").ToString()!;
    }

    /// <summary>
    /// Validate user credentials
    /// </summary>
    /// <param name="userName">user</param>
    /// <param name="password">password</param>
    /// <returns>Token for user</returns>
    /// <exception cref="NotImplementedException"></exception>
    public async Task<string> ValidAsync(string userName, string password)
    {
      string pass = _password.GetPassword(password);
      userName = userName.ToLower();
      var user = await _db.FindAsync(f => f.Usuario == userName && f.Password == pass).Result.FirstOrDefaultAsync();
      if (user != null)
      {
        var key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_secretKey));
        var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var authClaims = new List<Claim>
        {
          new Claim(ClaimTypes.Name, user.Nombre),
          new Claim(ClaimTypes.NameIdentifier, userName),
          new Claim(ClaimTypes.UserData, user.App),
          new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        };

        var token = new JwtSecurityToken(
            issuer: _issuer,
            audience: _audience,
            authClaims,
            expires: DateTime.UtcNow.AddDays(5),
            signingCredentials: signIn);
        return new JwtSecurityTokenHandler().WriteToken(token);
      }
      else
      {
        return "";
      }
    }
	}
}