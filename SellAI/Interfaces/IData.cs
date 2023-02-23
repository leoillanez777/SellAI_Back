using System;
using SellAI.Models;
using SellAI.Models.Data;
using SellAI.Models.DTOs;

namespace SellAI.Interfaces
{
  public interface IData
  {
    /// <summary>
    /// Create or update data in mongodb.
    /// </summary>
    /// <param name="datas">class datas</param>
    /// <param name="roleAppDTO">rol, name of app and user the security</param>
    /// <param name="create">create or update if it is false</param>
    /// <returns>response with code "ok"</returns>
    Task<Response> CreateOrUpdateAsync(Datas datas, RoleAppDTO roleAppDTO,bool create);
  }
}

