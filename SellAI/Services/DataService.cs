using System;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using SellAI.Interfaces;
using SellAI.Models;
using SellAI.Models.Data;
using SellAI.Models.DTOs;

namespace SellAI.Services
{
  public class DataService : IData
  {
    private readonly IMongoClient _client;
    private readonly IMongoCollection<Datas> _db;
    private readonly ISysLog _syslog;
    private readonly string _nameCol;

    public DataService(
      IMongoClient client,
      IOptions<ContextMongoDB> options,
      ISysLog sysLog
    )
    {
      _nameCol = options.Value.DataCollectionName;
      _client = client;
      _db = _client.GetDatabase(options.Value.DatabaseName).GetCollection<Datas>(_nameCol);
      _syslog = sysLog;
    }

    public async Task<Response> CreateOrUpdateAsync(Datas datas, RoleAppDTO roleAppDTO, bool create = true)
    {
      Response response = new() {
        code = "ok",
        error = "¡Se grabo correctamente!"
      };

      using (var session = await _client.StartSessionAsync()) {
        try {
          session.StartTransaction();
          
          if (create) {
            await _db.InsertOneAsync(datas);
            await _syslog.CreateAsync(datas, _nameCol, roleAppDTO);
          }
          else {
            await _db.ReplaceOneAsync(d => d.Id == datas.Id, datas);
            await _syslog.UpdateAsync(datas, _nameCol, roleAppDTO);
          }
          await session.CommitTransactionAsync();
        }
        catch (Exception ex) {
          await session.AbortTransactionAsync();
          response.code = "exception";
          response.error = ex.Message;
        }
        
      }

      return response;
    }
  }
}

