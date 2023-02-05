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
    public class CategoryService: ICategory
    { 

        private readonly IMongoClient _client;
        private readonly IMongoCollection<Category> _db;
        private readonly string urlCategory = "https://api.wit.ai/entities/product_category/keywords";
        private readonly IConfiguration _configuration;

        public CategoryService(IMongoClient client, IOptions<ContextMongoDB> options, IConfiguration configuration) 
        {
            _client = client;
            _configuration = configuration;
            _db = _client.GetDatabase(options.Value.DatabaseName).GetCollection<Category>(options.Value.CategoryCollectionName);
        }

        /// <summary>
        /// generate a BsonDocument for pipeline
        /// </summary>
        /// <param name="tipo">id filtro</param>
        /// <param name="atribute">filter category/param>
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
        /// Filter the list of Categories from the application and if it considers its status
        /// </summary>
        /// <param name="app">Name of aplication</param>
        /// <param name="isActive">filter active</param>
        /// <returns>List of Categories accordin the filters</returns>
        public async Task<List<Category>> GetListAsync(string app, bool isActive)
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
            var response = JsonSerializer.Deserialize<List<Category>>(result.ToJson());
            return response!;
        }

        /// <summary>
        /// Generate the json of a model
        /// </summary>
        /// <param name="cat">Object Category</param>
        /// <returns>Generates the json of a model for the body of the methods post and update</returns>
        private string jsonCategoryKeywords(Category cat)
        {
            Keywords keys = new Keywords();
            keys.keyword = cat.name;
            keys.synonyms = new List<string>();
            keys.synonyms.Add(cat.name);
            return JsonSerializer.Serialize(keys);
        }

        /// <summary>
        /// add a category in the colection's Category and insert a product_category/keyword
        /// </summary>
        /// <returns>Returns a message on the status of the operation  </returns>
        public async Task<string> PostAsync(CategoryDTO category)
        {
            var mensaje = "error";
            try
            {
                #region search category for name and app
                var config = new MapperConfiguration(cfg =>
                    cfg.CreateMap<CategoryDTO, Category>()
                );
                var mapper = new Mapper(config);
                var cat = mapper.Map<Category>(category);

                var pipeline = new[] {
                    match("string","name", category.name), match("string","app", category.app)
                  };
                var result = await _db.AggregateAsync<BsonDocument>(pipeline).Result.ToListAsync();
                #endregion search category for name and app
                if (result.Count == 0)
                {
                    #region insert new category
                    await _db.InsertOneAsync(cat);
                    #endregion insert new category

                    #region insert new product_category/keyword
                    var jsonKeys = jsonCategoryKeywords(cat);
                    var client = new RestClient(urlCategory + "?v=20221114");
                    RestApiService ras = new RestApiService(_configuration);
                    mensaje = await ras.CallPostAsync(jsonKeys, client);
                    #endregion insert new product_category/keyword

                    #region Error in wit.ai, update old category
                    if (mensaje != "ok")
                    {
                        var filter = Builders<Category>.Filter.Where(r => r.name == cat.name);
                        await _db.DeleteOneAsync(filter, new CancellationToken());
                    }
                    #endregion Error in wit.ai, update old category

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
        /// update a category in the colection's Category, delete the old category keyword and insert the new category keyword
        /// </summary>
        /// <param name="category">Category Object</param>
        /// <param name="id">Id Object</param>
        /// <returns>Returns a message on the status of the operation</returns>
        public async Task<string> UpdateAsync(CategoryDTO category, string id)
        {
            var mensaje = "error";
            try
            {
                #region search category for id
                var config = new MapperConfiguration(cfg =>
                    cfg.CreateMap<CategoryDTO, Category>()
                );
                var mapper = new Mapper(config);
                var cat = mapper.Map<Category>(category);
                
                var pipeline = new[] {
                        match("object","_id", id)
                      };
                var result = await _db.AggregateAsync<BsonDocument>(pipeline).Result.ToListAsync();
                foreach (var reg in result)
                    reg["_id"] = reg["_id"].AsObjectId.ToString();
                var response = JsonSerializer.Deserialize<List<Category>>(result.ToJson());
                var anterior = response.FirstOrDefault();
                #endregion search category for id

                if (result.Count == 1)
                {
                    #region Update Category
                    var filter = Builders<Category>.Filter.Where(r => r.Id == id);
                    var update = Builders<Category>.Update
                        .Set(p => p.name, cat.name)
                        .Set(p => p.description, cat.description)
                        .Set(p => p.isActive, cat.isActive);
                    await _db.UpdateOneAsync(filter, update);
                    #endregion Update Category

                    #region Delete old product_category/keyword
                    var getUrlEliminar = urlCategory + "/" + anterior.name + "?v=20221114";
                    var client = new RestClient(getUrlEliminar);
                    RestApiService ras = new RestApiService(_configuration);
                    mensaje = await ras.CallDeleteAsync(client);
                    #endregion Delete old product_category/keyword

                    #region Insert new product_category/keyword
                    string jsonKeys = jsonCategoryKeywords(cat);
                    client = new RestClient(urlCategory + "?v=20221114");
                    ras = new RestApiService(_configuration);
                    mensaje = await ras.CallPostAsync(jsonKeys, client);
                    #endregion Insert new product_category/keyword

                    #region Error in wit.ai, update old category
                    if (mensaje != "ok")
                    {
                        update = Builders<Category>.Update
                       .Set(p => p.app, anterior.app)
                       .Set(p => p.name, anterior.name)
                       .Set(p => p.description, anterior.description)
                       .Set(p => p.isActive, anterior.isActive);
                        await _db.UpdateOneAsync(filter, update);
                    }
                    #endregion Error in wit.ai, update old category

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
