using System;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Newtonsoft.Json;
using SellAI.Interfaces;
using SellAI.Models;
using SellAI.Models.DTOs;

namespace SellAI.Services
{
  public class LogService : ISysLog
  {

    private readonly IMongoClient _client;
    private readonly IMongoCollection<Sys_Log> _db;

    public LogService(IMongoClient client,
      IOptions<ContextMongoDB> options,
      IConfiguration configuration)
    {
      _client = client;
      _db = _client.GetDatabase(options.Value.DatabaseName).GetCollection<Sys_Log>(options.Value.SysLogCollectionName);
    }

    public async Task CreateAsync<T>(T obj, string col, RoleAppDTO dTO)
    {
      await SaveAsync(obj, "create", col, dTO);
    }

    public async Task SaveAsync<T>(T obj, string cmd, string col, RoleAppDTO dTO)
    {
      Sys_Log sysLog = new()
      {
        Datos = JsonConvert.SerializeObject(obj),
        Comando = cmd,
        Coleccion = col,
        App = dTO.App,
        Usuario = dTO.Usuario
      };
      await _db.InsertOneAsync(sysLog);
    }

    public async Task UpdateAsync<T>(T obj, string col, RoleAppDTO dTO)
    {
      await SaveAsync(obj, "update", col, dTO);
    }
  }
}

