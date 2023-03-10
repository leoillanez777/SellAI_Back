using System;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using Newtonsoft.Json;
using SellAI.Interfaces;
using SellAI.Middlewares.Exceptions;
using SellAI.Models;
using SellAI.Models.Data;
using SellAI.Models.DTOs;
using SellAI.Models.Objects;

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

      using var session = await _client.StartSessionAsync();
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
      catch (MongoCommandException ex) {
        await session.AbortTransactionAsync();
        response.code = "exception";
        response.error = ex.Message;
      } 

      return response;
    }

    public async Task<List<ReadDataDTO>> ReadAsync(List<ReadData> readDatas, string intent, string app)
    {
      try {
        List<BsonDocument> bsons = new();

        // Add $match for search only in specific intent and app.
        bsons.Add(new BsonDocument("$match", new BsonDocument {
          { "intent.name", BsonValue.Create(intent) },
          { "app", BsonValue.Create(app) }
        }));

        foreach (var c in readDatas) {
          var findCommand = bsons.FirstOrDefault(b => b.Names.Any(n => n.Contains(c.Command)));

          if (c.ExtraCmd is not null) {
            switch (c.ExtraCmd) {
              case "$regex":
                var regExpr = c.CondExtra == null ? $"/{c.Value}/i" : $"/{c.Value}/{c.CondExtra}";
                var bsonDocExp = new BsonDocument(c.ExtraCmd, new BsonRegularExpression(c.Value, c.CondExtra ?? "i"));
                if (findCommand is null) {
                  bsons.Add(new BsonDocument(c.Command, new BsonDocument(c.FullPath, bsonDocExp)));
                }
                else {
                  findCommand.GetValue(c.Command).AsBsonDocument.AddRange(new BsonDocument(c.FullPath, bsonDocExp));
                }
                break;
            }
          }
          else {
            if (findCommand is null) {
              bsons.Add(new BsonDocument(c.Command, new BsonDocument(c.FullPath, BsonValue.Create(c.Value))));
            }
            else {
              findCommand.GetValue(c.Command).AsBsonDocument.AddRange(new BsonDocument(c.FullPath, BsonValue.Create(c.Value)));
            }
          }
        }

        var result = await _db.AggregateAsync<Datas>(bsons).Result.ToListAsync();

        string jsonResult = JsonConvert.SerializeObject(result);

        var jsonSetting = new JsonSerializerSettings {
          MissingMemberHandling = MissingMemberHandling.Ignore
        };
        List<ReadDataDTO> dtos = JsonConvert.DeserializeObject<List<ReadDataDTO>>(jsonResult, jsonSetting)!;

        return dtos;
      }
      catch (JsonSerializationException ex) {
        throw new JsonSerializationException($"Se produjo un error al deserializar. Mensaje: {ex.Message}. Detalles: {ex.StackTrace}. Posición: {ex.LinePosition}");
      }
      catch (JsonException ex) {
        throw new Exception($"Se produjo un error al deserializar la respuesta de la API de mensajes: {ex.Message}. Detalles: {ex.StackTrace}");
      }
      catch (MongoQueryException ex) {
        throw new EMongoDBQuery("Error al consultar base de datos.", ex);
      }
    }
  }
}

