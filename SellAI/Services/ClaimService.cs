using System;
using System.Security.Claims;
using System.Security.Principal;
using Newtonsoft.Json;
using SellAI.Interfaces;
using SellAI.Models.Data;
using SellAI.Models.DTOs;

namespace SellAI.Services
{
  public class ClaimService : IClaim
  {
    public ClaimService()
    {
    }

    public RoleAppDTO GetRoleAndApp(IIdentity identity)
    {
      RoleAppDTO roleAppDTO = new RoleAppDTO();

      ClaimsIdentity? claimsIdentity = identity as ClaimsIdentity;

      if (claimsIdentity != null) {
        foreach (var claim in claimsIdentity.Claims) {
          switch (claim.Type) {
            case ClaimTypes.Role:
              roleAppDTO.Roles = JsonConvert.DeserializeObject<List<string>>(claim.Value)!;
              break;
            case ClaimTypes.UserData:
              roleAppDTO.App = claim.Value;
              break;
            case ClaimTypes.NameIdentifier:
              roleAppDTO.Usuario = claim.Value;
              break;
          }
        }
      }

      return roleAppDTO;
    }

    public string GetData(IIdentity identity, ClaimsTypeData claimsTypeData)
    {
      string types = ClaimTypes.Email;
      switch (claimsTypeData) {
        case ClaimsTypeData.UserID:
          types = ClaimTypes.NameIdentifier;
          break;
        case ClaimsTypeData.App:
          types = ClaimTypes.UserData;
          break;
        case ClaimsTypeData.Roles:
          types = ClaimTypes.Role;
          break;
      }

      ClaimsIdentity? claimsIdentity = identity as ClaimsIdentity;
      if (claimsIdentity != null) {
        var res = (from a in claimsIdentity.Claims
                 where a.Type == types
                 select a.Value).FirstOrDefault();

        return res!;
      }

      return "";
    }
  }
}

