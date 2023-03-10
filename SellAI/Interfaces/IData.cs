using System;
using SellAI.Models;
using SellAI.Models.Data;
using SellAI.Models.DTOs;
using SellAI.Models.Objects;

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

    /// <summary>
    /// Return data in mongodb
    /// </summary>
    /// <param name="readDatas">filter to apply</param>
    /// <param name="intent">intent to search</param>
    /// <param name="app">filter data for app name</param>
    /// <returns>data from mongodb</returns>
    Task<List<ReadDataDTO>> ReadAsync(List<ReadData> readDatas, string intent, string app);
  }
}

