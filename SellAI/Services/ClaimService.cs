using System;
using System.Security.Claims;
using Newtonsoft.Json;
using SellAI.Interfaces;
using SellAI.Models.DTOs;

namespace SellAI.Services
{
  public class ClaimService : IClaim
  {
    public ClaimService()
    {
    }

    /// <summary>
    /// Get only role and app from claims.
    /// </summary>
    /// <param name="identity"></param>
    /// <returns></returns>
    public RoleAppDTO GetRoleAndApp(ClaimsIdentity identity)
    {
      RoleAppDTO roleAppDTO = new RoleAppDTO();
      foreach (var claim in identity.Claims) {
        switch (claim.Type) {
          case ClaimTypes.Role:
            roleAppDTO.Roles = JsonConvert.DeserializeObject<List<string>>(claim.Value)!;
            break;
          case ClaimTypes.UserData:
            roleAppDTO.App = claim.Value;
            break;
        }
      }

      return roleAppDTO;
    }

    public string GetApp(ClaimsIdentity identity)
    {
      var app = (from a in identity.Claims
                 where a.Type == ClaimTypes.UserData
                 select a.Value).FirstOrDefault();

      return app!;
    }

    public List<string> GetRoles(ClaimsIdentity identity)
    {
      var app = (from a in identity.Claims
                 where a.Type == ClaimTypes.Role
                 select a.Value).ToList();

      return app;
    }
  }
}

