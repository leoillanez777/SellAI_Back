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
  }
}

