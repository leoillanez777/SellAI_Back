﻿using System;
using System.Security.Claims;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using SellAI.Interfaces;
using SellAI.Middlewares.Exceptions;
using SellAI.Models;
using SellAI.Models.DTOs;

namespace SellAI.Services;

public class SysContextService : ISysContext {
  private readonly IMongoClient _client;
  private readonly IMongoCollection<Sys_Context> _db;

  public SysContextService(IMongoClient client, IOptions<ContextMongoDB> options)
  {
    _client = client;
    _db = _client.GetDatabase(options.Value.DatabaseName).GetCollection<Sys_Context>(options.Value.SysContextCollectionName);
  }

  public async Task<Sys_Context> GetContextAsync(string Id)
  {
    return await _db.FindSync(f => f.Id == Id).FirstOrDefaultAsync();
  }

  public async Task<Sys_Context> CreateContextAsync(Sys_Context sys_Context)
  {
    await _db.InsertOneAsync(sys_Context);
    return sys_Context;
  }

  public async Task<Sys_Context> UpdateContextAsync(Sys_Context sys_Context)
  {
    try {
      await _db.ReplaceOneAsync(s => s.Id == sys_Context.Id, sys_Context);
      return sys_Context;
    }
    catch (MongoCommandException ex) {
      throw new EMongoDBCommand("Error al consultar base de datos.", ex);
    }
  }
}


