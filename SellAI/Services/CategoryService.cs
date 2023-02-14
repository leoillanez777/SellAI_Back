using AutoMapper;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using NuGet.Protocol;
using RestSharp;
using SellAI.Interfaces;
using SellAI.Models;
using SellAI.Models.AI.Objects;
using SellAI.Models.DTOs;
using SellAI.Models.Objects;
using System.Text.Json;

namespace SellAI.Services
{
  public class CategoryService : ICategory {

    private readonly IMongoClient _client;
    private readonly IMongoCollection<Category> _db;
    private readonly IConfiguration _configuration;
    private readonly IRestApi _restApi;
    private readonly IMapper _mapper;
    private readonly string _entityName = "product_category";

    public CategoryService(IMongoClient client,
      IOptions<ContextMongoDB> options,
      IConfiguration configuration,
      IRestApi restApi,
      IMapper map)
    {
      _client = client;
      _configuration = configuration;
      _db = _client.GetDatabase(options.Value.DatabaseName).GetCollection<Category>(options.Value.CategoryCollectionName);
      _restApi = restApi;
      _mapper = map;
    }

    /// <summary>
    /// Filter the list of Categories from the application and if it considers its status
    /// </summary>
    /// <param name="app">Name of aplication</param>
    /// <param name="isActive">filter active</param>
    /// <returns>List of Categories accordin the filters</returns>
    public async Task<List<Category>> GetListAsync(string app, bool isActive)
    {
      var response = await _db.FindAsync(c => c.App == app && c.Activo == isActive).Result.ToListAsync();
      return response;
    }

    /// <summary>
    /// add a category in the collection's Category and insert a product_category/keyword
    /// </summary>
    /// <returns>Returns a message on the status of the operation  </returns>
    public async Task<string> PostAsync(CategoryDTO category, string app)
    {
      // tengo que agregar datos de categoria.
      // tambien agregar si tiene sinonimos.
      // UNDONE: probar crear entity existentes.
      var mensaje = "error";
      try {
        var result = await _db.FindAsync(c => c.Nombre.ToLower().Contains(category.Nombre.ToLower()) && c.App == app).Result.ToListAsync();
        if (result.Count == 0) {
          Category cat = new();
          cat = _mapper.Map<Category>(category);
          cat.App = app;
          using (var session = await _client.StartSessionAsync()) {
            try {

              await _db.InsertOneAsync(cat);
              var jsonKeys = jsonKeywords(cat);

              mensaje = await _restApi.CallPostEntityAsync(jsonKeys, _entityName);

              if (mensaje != "error")
                await session.CommitTransactionAsync();
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

    /// <summary>
    /// update a category in the colection's Category, delete the old category keyword and insert the new category keyword
    /// </summary>
    /// <param name="category">Category Object</param>
    /// <param name="id">Id Object</param>
    /// <returns>Returns a message on the status of the operation</returns>
    public async Task<string> UpdateAsync(CategoryDTO category, string id, string app)
    {
      var mensaje = "error";

      using (var session = await _client.StartSessionAsync()) {
        try {
          Category cat = new();
          cat = _mapper.Map<Category>(category);
          cat.App = app;

          await _db.ReplaceOneAsync(c => c.Id == id && c.App == app, cat);

          #region Delete old product_category/keyword
          mensaje = await _restApi.CallDeleteEntityAsync(_entityName, category.Nombre);
          #endregion Delete old product_category/keyword

          #region Insert new product_category/keyword
          string jsonKeys = jsonKeywords(cat);
          mensaje = await _restApi.CallPostEntityAsync(jsonKeys, _entityName);
          #endregion Insert new product_category/keyword

          if (mensaje != "error")
            await session.CommitTransactionAsync();
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
      return JsonSerializer.Serialize(keys);
    }
  }
}
