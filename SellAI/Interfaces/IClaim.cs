using System;
using System.Security.Claims;
using System.Security.Principal;
using SellAI.Models.Data;
using SellAI.Models.DTOs;

namespace SellAI.Interfaces
{
  public interface IClaim
  {
    /// <summary>
    /// Get roles, name of app and user id from claims.
    /// </summary>
    /// <param name="identity">identity from authorize</param>
    /// <returns>return class with data.</returns>
    RoleAppDTO GetRoleAndApp(IIdentity identity);

    /// <summary>
    /// Get data of claims
    /// </summary>
    /// <param name="identity">data of token</param>
    /// <param name="claimsTypeData">type of claims to return</param>
    /// <returns>data of claim to return or empty it is null</returns>
    string GetData(IIdentity identity, ClaimsTypeData claimsTypeData);
  }
}

