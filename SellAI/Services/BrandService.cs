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

namespace SellAI.Services
{
  public class BrandService : IBrand {

    private readonly IMongoClient _client;
    private readonly IMongoCollection<Brand> _db;
    private readonly IRestApi _restApi;
    private readonly ISysLog _syslog;
    private readonly IMapper _mapper;
    private readonly string _entityName = "product_brand";
    private readonly string _nameCol;

    public BrandService(IMongoClient client,
      IOptions<ContextMongoDB> options,
      IRestApi restApi,
      ISysLog sysLog,
      IMapper map)
    {
      _nameCol = options.Value.BrandCollectionName;
      _client = client;
      _db = _client.GetDatabase(options.Value.DatabaseName).GetCollection<Brand>(_nameCol);
      _restApi = restApi;
      _syslog = sysLog;
      _mapper = map;
    }

    public async Task<List<BrandDTO>> GetListAsync(RoleAppDTO roleAppDTO, bool isActive)
    {
      var response = await _db.FindAsync(c => c.App == roleAppDTO.App && c.Activo == isActive).Result.ToListAsync();
      List<BrandDTO> brands = new();
      response.ForEach(bra => {
        BrandDTO brand = _mapper.Map<BrandDTO>(bra);
        brands.Add(brand);
      });
      return brands;
    }

    public async Task<string> PostAsync(BrandDTO brandDTO, RoleAppDTO roleAppDTO)
    {
      var mensaje = "error";
      try {
        var result = await _db.FindAsync(c => c.Nombre.ToLower().Contains(brandDTO.Nombre.ToLower()) && c.App == roleAppDTO.App).Result.ToListAsync();
        if (result.Count == 0) {
          Brand brand = new();
          brand = _mapper.Map<Brand>(brandDTO);
          brand.App = roleAppDTO.App;
          using (var session = await _client.StartSessionAsync()) {
            try {
              session.StartTransaction();
              await _db.InsertOneAsync(brand);

              await _syslog.CreateAsync(brand, _nameCol, roleAppDTO);

              var jsonKeys = jsonKeywords(brand);

              mensaje = await _restApi.CallPostEntityAsync(jsonKeys, _entityName);

              if (mensaje != "error" || mensaje == "already exists") {
                await session.CommitTransactionAsync();
                // for return data in json.
                brandDTO.Id = brand.Id!;
                mensaje = JsonConvert.SerializeObject(brandDTO);
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

    
    public async Task<string> UpdateAsync(BrandDTO brandDTO, RoleAppDTO roleAppDTO)
    {
      var mensaje = "error";

      using (var session = await _client.StartSessionAsync()) {
        try {
          Brand brand = new();
          brand = _mapper.Map<Brand>(brandDTO);
          brand.App = roleAppDTO.App;

          session.StartTransaction();
          await _db.ReplaceOneAsync(c => c.Id == brandDTO.Id && c.App == roleAppDTO.App, brand);

          await _syslog.UpdateAsync(brand, _nameCol, roleAppDTO);

          #region Delete old product_category/keyword
          mensaje = await _restApi.CallDeleteEntityAsync(_entityName, brandDTO.Nombre);
          #endregion Delete old product_category/keyword

          #region Insert new product_category/keyword
          string jsonKeys = jsonKeywords(brand);
          mensaje = await _restApi.CallPostEntityAsync(jsonKeys, _entityName);
          #endregion Insert new product_category/keyword

          if (mensaje != "error") {
            await session.CommitTransactionAsync();
            // for return data in json.
            brandDTO.Id = brand.Id!;
            mensaje = JsonConvert.SerializeObject(brandDTO);
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

    public Task<string> DeleteAsync(string id, RoleAppDTO claims)
    {
      throw new NotImplementedException();
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
      return System.Text.Json.JsonSerializer.Serialize(keys);
    }

  }
}
