using System;
using AutoMapper;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Newtonsoft.Json;
using SellAI.Interfaces;
using SellAI.Middlewares.Exceptions;
using SellAI.Models;
using SellAI.Models.AI;
using SellAI.Models.DTOs;

namespace SellAI.Services
{
  public class SysMenuService : ISysMenu
  {
    private readonly IMongoClient _client;
    private readonly IMongoCollection<Sys_Menu> _db;
    private readonly IMapper _mapper;

    public SysMenuService(
      IMongoClient client,
      IOptions<ContextMongoDB> options,
      IMapper map
    )
    {
      _client = client;
      _db = _client.GetDatabase(options.Value.DatabaseName).GetCollection<Sys_Menu>(options.Value.SysMenuCollectionName);
      _mapper = map;
    }

    public async Task<Sys_Menu> GetIntentAsync(string intentName, RoleAppDTO roleApp)
    {
      try {
        var sys_menu = await _db.FindAsync(f => f.Nombre == intentName && (f.App == roleApp.App || f.App == "")).Result.FirstOrDefaultAsync();
        return sys_menu;
      }
      catch (MongoQueryException ex) {
        throw new EMongoDBQuery("Error al consultar base de datos.", ex);
      }
    }

    public async Task<Sys_Menu?> GetAsync(string intentId, RoleAppDTO roleApp)
    {
      try {
        var sys_menu = await _db.FindAsync(f => f.IntentID == intentId && (f.App == roleApp.App || f.App == "")).Result.FirstOrDefaultAsync();
        return sys_menu;
      }
      catch (EMongoDBQuery ex) {
        throw new EMongoDBQuery("Error al recuperar datos: ", ex);
      }
    }

    public async Task<string> PostAsync(Sys_MenuDTO sysMenuDTO, RoleAppDTO claims)
    {
      try {
        Sys_Menu sys_Menu = new();
        sys_Menu = _mapper.Map<Sys_Menu>(sysMenuDTO);
        sys_Menu.App = claims.App;
        await _db.InsertOneAsync(sys_Menu);
        return JsonConvert.SerializeObject(sys_Menu);
      }
      catch (EMongoDBCommand ex) {
        throw new EMongoDBCommand($"Error al grabar Intent: {ex.Message}", ex);
      }
      catch (Exception ex) {
        throw new Exception($"Error no controlado en grabar Intent: {ex.Message}", ex);
      }
    }

    public async Task<string> UpdateAsync(Sys_Menu sysMenuDTO, RoleAppDTO claims)
    {
      return "";
    }

    public async Task<string> DeleteAsync(string id, RoleAppDTO claims)
    {
      return "";
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

