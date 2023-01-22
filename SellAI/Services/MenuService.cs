using System;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using Newtonsoft.Json;
using MongoDB.Bson.Serialization;
using SellAI.Interfaces;
using SellAI.Models;
using SellAI.Models.Objects;

namespace SellAI.Services
{
  public class MenuService : IUserMenu
  {
    private readonly IMongoClient _client;
    private readonly IMongoCollection<User_Menu> _db;

    public MenuService(IMongoClient client, IOptions<ContextMongoDB> options)
    {
      _client = client;
      _db = _client.GetDatabase(options.Value.DatabaseName).GetCollection<User_Menu>(options.Value.UserMenuCollectionName);
    }

    /// <summary>
    /// get menu according to roles
    /// </summary>
    /// <param name="roles">List of roles</param>
    /// <returns>filter menu according role</returns>
    public async Task<List<User_Menu>> GetMenuAsync(List<string> roles)
    {
      // rol to allow access all users
      roles.Add("*");

      var rolesBson = roles.Select(x => new BsonString(x)).ToList();
      var match = new BsonDocument("$match", new BsonDocument("roles", new BsonDocument("$in", new BsonArray(rolesBson))));
      var pipeline = new[] {
        match
      };
      var result = await _db.AggregateAsync<BsonDocument>(pipeline).Result.ToListAsync();

      foreach(var reg in result) {
        reg["_id"] = reg["_id"].AsObjectId.ToString();

        var itemsReg = reg.GetValue("items", null).AsBsonArray;
        ManipulateItems(ref itemsReg, rolesBson);
      }

      var response = JsonConvert.DeserializeObject<List<User_Menu>>(result.ToJson());
      return response!;
    }


    /// <summary>
    /// Recursive manipulate items of menu.
    /// </summary>
    /// <param name="array">items of menu for reference</param>
    /// <param name="roles">list of roles</param>
    private void ManipulateItems(ref BsonArray array, List<BsonString> roles)
    {
      // if array isn't null
      if (array != null && array.Count > 0) {
        var deleteItems = new BsonArray();

        // loop items of menu.
        foreach (var ar in array) {
          var irRoles = ar.AsBsonDocument.GetValue("roles", new BsonArray()).AsBsonArray;
          var irItems = ar.AsBsonDocument.GetValue("items", new BsonArray()).AsBsonArray;

          if (irItems != null && irItems.Count > 0)
            ManipulateItems(ref irItems, roles);
          if (!irRoles.Intersect(roles).Any()) {
            deleteItems.Add(ar);
          }
        }

        // delete items without role.
        foreach (var delItem in deleteItems) {
          array.Remove(delItem.AsBsonValue);
        }
      }
    }
  }
}

