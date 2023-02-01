using System;
using System.Security.Claims;
using SellAI.Models.DTOs;

namespace SellAI.Interfaces
{
  public interface IClaim
  {
    RoleAppDTO GetRoleAndApp(ClaimsIdentity identity);
    string GetApp(ClaimsIdentity identity);
    List<string> GetRoles(ClaimsIdentity identity);
  }
}

