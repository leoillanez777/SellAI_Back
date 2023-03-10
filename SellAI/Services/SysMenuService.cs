﻿using System;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using SellAI.Interfaces;
using SellAI.Models;
using SellAI.Models.AI;
using SellAI.Models.DTOs;

namespace SellAI.Services
{
  public class SysMenuService : ISysMenu
  {
    private readonly IMongoClient _client;
    private readonly IMongoCollection<Sys_Menu> _db;

    public SysMenuService(
      IMongoClient client,
      IOptions<ContextMongoDB> options
    )
    {
      _client = client;
      _db = _client.GetDatabase(options.Value.DatabaseName).GetCollection<Sys_Menu>(options.Value.SysMenuCollectionName);
    }

    public async Task<Sys_Menu> GetIntentAsync(string intentName, RoleAppDTO roleApp)
    {
      try {
        var sys_menu = await _db.FindAsync(f => f.Nombre == intentName && f.App == roleApp.App).Result.FirstOrDefaultAsync();
        return sys_menu;
      }
      catch (Exception ex) {
        throw new Exception(ex.Message);
      }
    }

    public async Task<string> GetOutOfScopeAsync()
    {
      string response = "";
      var sys_menu = await _db.FindAsync(f => f.IntentID == "outofscope").Result.FirstOrDefaultAsync();
      if (sys_menu != null)
        response = sys_menu.Mensaje!;
      return response;
    }

    public async Task<Sys_Menu> GetGreetingAsync(string nameIntent)
    {
      Sys_Menu response = null!;
      if (nameIntent.ToLower() == "saludos")
        response = await _db.FindAsync(f => f.Nombre == "saludos" || f.IntentID == "1275683856349570").Result.FirstOrDefaultAsync();
      return response;
    }
  }
}
