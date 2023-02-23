using System.Drawing.Drawing2D;
using AutoMapper;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using Newtonsoft.Json;
using NuGet.Protocol;
using RestSharp;
using SellAI.Interfaces;
using SellAI.Models;
using SellAI.Models.AI.Objects;
using SellAI.Models.DTOs;
using SellAI.Models.Objects;

namespace SellAI.Services
{
  public class CategoryService : ICategory {

    private readonly IMongoClient _client;
    private readonly IMongoCollection<Category> _db;
    private readonly IRestApi _restApi;
    private readonly ISysLog _syslog;
    private readonly IMapper _mapper;
    private readonly string _entityName = "product_category";
    private readonly string _nameCol;

    public CategoryService(IMongoClient client,
      IOptions<ContextMongoDB> options,
      IRestApi restApi,
      ISysLog sysLog,
      IMapper map)
    {
      _nameCol = options.Value.CategoryCollectionName;
      _client = client;
      _db = _client.GetDatabase(options.Value.DatabaseName).GetCollection<Category>(_nameCol);
      _restApi = restApi;
      _syslog = sysLog;
      _mapper = map;
    }

    public async Task<List<CategoryDTO>> GetListAsync(RoleAppDTO roleAppDTO, bool isActive)
    {
      var response = await _db.FindAsync(c => c.App == roleAppDTO.App && c.Activo == isActive).Result.ToListAsync();
      List<CategoryDTO> categories = new();
      response.ForEach(cat => {
        CategoryDTO category = _mapper.Map<CategoryDTO>(cat);
        categories.Add(category);
      });
      return categories;
    }

    public async Task<string> PostAsync(CategoryDTO categoryDTO, RoleAppDTO roleAppDTO)
    {
      var mensaje = "error";
      try {
        var result = await _db.FindAsync(c => c.Nombre.ToLower().Contains(categoryDTO.Nombre.ToLower()) && c.App == roleAppDTO.App).Result.ToListAsync();
        if (result.Count == 0) {
          Category cat = new();
          cat = _mapper.Map<Category>(categoryDTO);
          cat.App = roleAppDTO.App;
          using (var session = await _client.StartSessionAsync()) {
            try {
              session.StartTransaction();
              await _db.InsertOneAsync(cat);

              await _syslog.CreateAsync(cat, _nameCol, roleAppDTO);

              var jsonKeys = jsonKeywords(cat);

              mensaje = await _restApi.CallPostEntityAsync(jsonKeys, _entityName);

              if (mensaje != "error" || mensaje == "already exists") {
                // FIXME: To add remaining keywords if mensaje is "already exists".
                await session.CommitTransactionAsync();

                categoryDTO.Id = cat.Id!;
                mensaje = JsonConvert.SerializeObject(categoryDTO);
              }
              else
                await session.AbortTransactionAsync();
            }
            catch (Exception ex) {
              await session.AbortTransactionAsync();
              throw new Exception(ex.Message);
            } 
          }
        }
        else if (result.Count == 1)
          mensaje = "duplicado";
        else
          mensaje = "error";

      }
      catch (Exception ex) {
        throw new Exception(ex.Message);
      }
      return mensaje;
    }

    public async Task<string> UpdateAsync(CategoryDTO categoryDTO, RoleAppDTO roleAppDTO)
    {
      var mensaje = "error";

      using (var session = await _client.StartSessionAsync()) {
        try {
          Category cat = new();
          cat = _mapper.Map<Category>(categoryDTO);
          cat.App = roleAppDTO.App;

          session.StartTransaction();
          await _db.ReplaceOneAsync(c => c.Id == categoryDTO.Id && c.App == roleAppDTO.App, cat);

          await _syslog.UpdateAsync(cat, _nameCol, roleAppDTO);

          #region Delete old product_category/keyword
          mensaje = await _restApi.CallDeleteEntityAsync(_entityName, categoryDTO.Nombre);
          #endregion Delete old product_category/keyword

          #region Insert new product_category/keyword
          string jsonKeys = jsonKeywords(cat);
          mensaje = await _restApi.CallPostEntityAsync(jsonKeys, _entityName);
          #endregion Insert new product_category/keyword

          if (mensaje != "error") {
            await session.CommitTransactionAsync();

            categoryDTO.Id = cat.Id!;
            mensaje = JsonConvert.SerializeObject(categoryDTO);
          }
          else
            await session.AbortTransactionAsync();

        }
        catch (Exception ex) {
          await session.AbortTransactionAsync();
          throw new Exception(ex.Message);
        }
      }
      return mensaje;
    }

    public Task<string> DeleteAsync(string id, RoleAppDTO roleAppDTO)
    {
      // Delete on DB, but verify if in wit.ai.
      // Create Log.
      throw new NotImplementedException();
    }

    /// <summary>
    /// Generate the json of a model
    /// </summary>
    /// <param name="cat">Object Category</param>
    /// <returns>Generates the json of a model for the body of the methods post and update</returns>
    private string jsonKeywords(Category cat)
    {
      Keywords keys = new();
      keys.keyword = cat.Nombre;
      keys.synonyms = cat.Sinonimos;
      if (cat.Sinonimos.Count == 0)
        keys.synonyms.Add(cat.Nombre);
      return System.Text.Json.JsonSerializer.Serialize(keys);
    }
  }
}
