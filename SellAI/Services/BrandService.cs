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
using System.Text.Json;

namespace SellAI.Services
{
  public class BrandService : IBrand {

    private readonly IMongoClient _client;
    private readonly IMongoCollection<Brand> _db;
    private readonly IConfiguration _configuration;
    private readonly IRestApi _restApi;
    private readonly IMapper _mapper;
    private readonly string _entityName = "product_brand";

    public BrandService(IMongoClient client,
      IOptions<ContextMongoDB> options,
      IConfiguration configuration,
      IRestApi restApi,
      IMapper map)
    {
      _client = client;
      _configuration = configuration;
      _db = _client.GetDatabase(options.Value.DatabaseName).GetCollection<Brand>(options.Value.BrandCollectionName);
      _restApi = restApi;
      _mapper = map;
    }

    public async Task<List<Brand>> GetListAsync(string app, bool isActive)
    {
      var response = await _db.FindAsync(c => c.App == app && c.Activo == isActive).Result.ToListAsync();
      return response;
    }

    public async Task<string> PostAsync(BrandDTO brandDTO, string app)
    {
      var mensaje = "error";
      try {
        var result = await _db.FindAsync(c => c.Nombre.ToLower().Contains(brandDTO.Nombre.ToLower()) && c.App == app).Result.ToListAsync();
        if (result.Count == 0) {
          Brand brand = new();
          brand = _mapper.Map<Brand>(brandDTO);
          brand.App = app;
          using (var session = await _client.StartSessionAsync()) {
            try {

              await _db.InsertOneAsync(brand);
              var jsonKeys = jsonKeywords(brand);

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

    
    public async Task<string> UpdateAsync(BrandDTO brandDTO, string id, string app)
    {
      var mensaje = "error";

      using (var session = await _client.StartSessionAsync()) {
        try {
          Brand brand = new();
          brand = _mapper.Map<Brand>(brandDTO);
          brand.App = app;

          await _db.ReplaceOneAsync(c => c.Id == id && c.App == app, brand);

          #region Delete old product_category/keyword
          mensaje = await _restApi.CallDeleteEntityAsync(_entityName, brandDTO.Nombre);
          #endregion Delete old product_category/keyword

          #region Insert new product_category/keyword
          string jsonKeys = jsonKeywords(brand);
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
    private string jsonKeywords(Brand brand)
    {
      Keywords keys = new();
      keys.keyword = brand.Nombre;
      keys.synonyms = brand.Sinonimos;
      if (brand.Sinonimos.Count == 0)
        keys.synonyms.Add(brand.Nombre);
      return JsonSerializer.Serialize(keys);
    }

  }
}
