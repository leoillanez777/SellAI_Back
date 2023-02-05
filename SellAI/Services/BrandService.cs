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
    public class BrandService: IBrand
    { 

        private readonly IMongoClient _client;
        private readonly IMongoCollection<Brand> _db;
        private readonly IConfiguration _configuration;
        private readonly IRestApi _resApi;

        public BrandService(IMongoClient client, IOptions<ContextMongoDB> options, IConfiguration configuration, IRestApi restApi) 
        {
            _client = client;
            _configuration = configuration;
            _db = _client.GetDatabase(options.Value.DatabaseName).GetCollection<Brand>(options.Value.BrandCollectionName);
            _resApi= restApi;
        }

        /// <summary>
        /// generate a BsonDocument for pipeline
        /// </summary>
        /// <param name="tipo">id filtro</param>
        /// <param name="atribute">filter brand/param>
        /// <param name="data">data for search/param>
        /// <returns>return a BsonDocument</returns>
        private BsonDocument match(string tipo, string atribute, object data)
        {
            switch (tipo) {
                case "string":
                    return new BsonDocument("$match", new BsonDocument(atribute, new BsonString(Convert.ToString(data))));
                case "bool":
                    return new BsonDocument("$match", new BsonDocument(atribute, new BsonBoolean(Convert.ToBoolean(data))));
                case "object":
                    return new BsonDocument("$match", new BsonDocument(atribute, ObjectId.Parse(data.ToString())));
                default:
                    return new BsonDocument();
            }
        }

        /// <summary>
        /// Generate the json of a model
        /// </summary>
        /// <param name="brand">Object Brand</param>
        /// <returns>Generates the json of a model for the body of the methods post and update</returns>
        private string jsonKeywords(Brand brand)
        {
            Keywords keys = new Keywords();
            keys.keyword = brand.name;
            keys.synonyms = new List<string>();
            keys.synonyms.Add(brand.name);
            return JsonSerializer.Serialize(keys);
        }

        /// <summary>
        /// Filter the list of Brands from the application and if it considers its status
        /// </summary>
        /// <param name="app">Name of aplication</param>
        /// <param name="isActive">filter active</param>
        /// <returns>List of Brands accordin the filters</returns>
        public async Task<List<Brand>> GetListAsync(string app, bool isActive)
        {
            var pipeline = new[] {
                    match("string","app", app)
                  };
            if(app == "")
                pipeline = new[] {
                    match("bool","isActive", isActive)
                  };
            else if (isActive)
                pipeline = new[] {
                     match("string","app", app), match("bool","isActive", isActive)
                  };
            var result = await _db.AggregateAsync<BsonDocument>(pipeline).Result.ToListAsync();

            foreach (var reg in result)
                reg["_id"] = reg["_id"].AsObjectId.ToString();
            var response = JsonSerializer.Deserialize<List<Brand>>(result.ToJson());
            return response!;
        }



        /// <summary>
        /// add a brand in the colection's Brand and insert a product_brand/keyword
        /// </summary>
        /// <returns>Returns a message on the status of the operation  </returns>
        public async Task<string> PostAsync(BrandDTO brand)
        {
            var mensaje = "error";
            try
            {
                #region search brand for name and app
                var config = new MapperConfiguration(cfg =>
                    cfg.CreateMap<BrandDTO, Brand>()
                );
                var mapper = new Mapper(config);
                var br = mapper.Map<Brand>(brand);

                var pipeline = new[] {
                    match("string","name", brand.name), match("string","app", brand.app)
                  };
                var result = await _db.AggregateAsync<BsonDocument>(pipeline).Result.ToListAsync();
                #endregion search brand for name and app
                if (result.Count == 0)
                {
                    #region insert new brand
                    await _db.InsertOneAsync(br);
                    #endregion insert new brand

                    #region insert new product_brand/keyword
                    var jsonKeys = jsonKeywords(br);
                    mensaje = await _resApi.CallPostBrandAsync(jsonKeys);
                    #endregion insert new product_brand/keyword

                    #region Error in wit.ai, update old brand
                    if (mensaje != "ok")
                    {
                        var filter = Builders<Brand>.Filter.Where(r => r.name == br.name);
                        await _db.DeleteOneAsync(filter, new CancellationToken());
                    }
                    #endregion Error in wit.ai, update old brand

                }
                else if (result.Count == 1)
                    mensaje = "duplicado";
                else
                    mensaje = "error";

            }
            catch(Exception ex)
            {
                
            }
            return mensaje;
        }


        /// <summary>
        /// update a brand in the colection's Brand, delete the old brand keyword and insert the new brand keyword
        /// </summary>
        /// <param name="brand">Brand Object</param>
        /// <param name="id">Id Object</param>
        /// <returns>Returns a message on the status of the operation</returns>
        public async Task<string> UpdateAsync(BrandDTO brand, string id)
        {
            var mensaje = "error";
            try
            {
                #region search brand for id
                var config = new MapperConfiguration(cfg =>
                    cfg.CreateMap<BrandDTO, Brand>()
                );
                var mapper = new Mapper(config);
                var br = mapper.Map<Brand>(brand);
                
                var pipeline = new[] {
                        match("object","_id", id)
                      };
                var result = await _db.AggregateAsync<BsonDocument>(pipeline).Result.ToListAsync();
                foreach (var reg in result)
                    reg["_id"] = reg["_id"].AsObjectId.ToString();
                var response = JsonSerializer.Deserialize<List<Brand>>(result.ToJson());
                var anterior = response.FirstOrDefault();
                #endregion search brand for id

                if (result.Count == 1)
                {
                    #region Update Brand
                    var filter = Builders<Brand>.Filter.Where(r => r.Id == id);
                    var update = Builders<Brand>.Update
                        .Set(p => p.name, br.name)
                        .Set(p => p.description, br.description)
                        .Set(p => p.isActive, br.isActive);
                    await _db.UpdateOneAsync(filter, update);
                    #endregion Update Brand

                    #region Delete old product_brand/keyword
                    mensaje = await _resApi.CallDeleteBrandAsync(anterior.name);
                    #endregion Delete old product_brand/keyword

                    #region Insert new product_brand/keyword
                    string jsonKeys = jsonKeywords(br);
                    mensaje = await _resApi.CallPostBrandAsync(jsonKeys);
                    #endregion Insert new product_brand/keyword

                    #region Error in wit.ai, update old brand
                    if (mensaje != "ok")
                    {
                        update = Builders<Brand>.Update
                       .Set(p => p.app, anterior.app)
                       .Set(p => p.name, anterior.name)
                       .Set(p => p.description, anterior.description)
                       .Set(p => p.isActive, anterior.isActive);
                        await _db.UpdateOneAsync(filter, update);
                    }
                    #endregion Error in wit.ai, update old brand

                }
                else
                    mensaje = "no encontrado";
            }
            catch (Exception ex)
            {

            }
            return mensaje;
        }

    }
}
