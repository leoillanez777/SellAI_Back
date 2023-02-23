using System;
using SellAI.Models.DTOs;

namespace SellAI.Interfaces
{
  public interface ISysLog
  {

    /// <summary>
    /// Generate new record of log.
    /// </summary>
    /// <param name="obj">class to save</param>
    /// <param name="cmd">create|read|update|delete</param>
    /// <param name="col">name of collection</param>
    /// <param name="dTO">data of claims</param>
    /// <returns></returns>
    Task SaveAsync<T>(T obj, string cmd, string col, RoleAppDTO dTO);

    /// <summary>
    /// Generate new record of log of create.
    /// </summary>
    /// <param name="obj">class to save</param>
    /// <param name="col">name of collection</param>
    /// <param name="dTO">data of claims</param>
    /// <returns></returns>
    Task CreateAsync<T>(T obj, string col, RoleAppDTO dTO);

    /// <summary>
    /// Generate new record of log of update.
    /// </summary>
    /// <param name="obj">class to save</param>
    /// <param name="col">name of collection</param>
    /// <param name="dTO">data of claims</param>
    /// <returns></returns>
    Task UpdateAsync<T>(T obj, string col, RoleAppDTO dTO);
  }
}

