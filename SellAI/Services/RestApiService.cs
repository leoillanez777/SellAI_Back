using System;
using Microsoft.DotNet.Scaffolding.Shared.CodeModifier.CodeChange;
using Newtonsoft.Json;
using Org.BouncyCastle.Asn1.Crmf;
using RestSharp;
using SellAI.Interfaces;
using SellAI.Models.AI;
using SellAI.Models.AI.Objects;
using SellAI.Models.Data;
using SellAI.Models.Objects;
using static Microsoft.Extensions.Logging.EventSource.LoggingEventSource;

namespace SellAI.Services
{
  public class RestApiService : IRestApi
  {
    private readonly string _secureKey;
    private readonly string _urlMsg;
    private readonly string _urlEntity;
    private readonly string _version = DateTime.UtcNow.ToString("yyyyMMdd");

    public RestApiService(IConfiguration configuration)
    {
      var wit = configuration.GetSection("Wit.ai");
      _secureKey = wit.GetValue<string>("Security")!;
      _urlMsg = wit.GetValue<string>("UrlMessage")!;
      _urlEntity = wit.GetValue<string>("UrlEntity")!;
    }
    
    /// <summary>
    /// Get intent and entities from message
    /// </summary>
    /// <param name="message">text to send</param>
    /// <returns>Model Message</returns>
    /// <exception cref="Exception"></exception>
    public async Task<Message> MessageAsync(string message)
    {
      Message msg = new();
      try {
        var client = new RestClient($"{_urlMsg}?v={_version}&q={message}");
        var request = new RestRequest();
        request.AddHeader("Authorization", _secureKey);
        var response = await client.GetAsync(request);

        if (response != null && response.Content != null) {
          msg = JsonConvert.DeserializeObject<Message>(response.Content)!;
        }
      }
      catch (Exception ex) {
        throw new Exception(ex.Message);
      }

      return msg;
    }

    #region Call Post Entity
    /// <summary>
    /// Create a new synonym for a keywords entity
    /// </summary>
    /// <param name="body">{"synonym":"Camembert city"}</param>
    /// <param name="entity">name of entity</param>
    /// <param name="keyword">name of keyword</param>
    /// <returns></returns>
    public async Task<string> CallPostEntityAsync(string body, string entity, string keyword)
    {
      string url = $"{_urlEntity}/{entity}/keywords/{keyword}/synonyms?v={_version}";
      return await PostOrDeleteAsync(url, body);
    }

    /// <summary>
    /// Add new values to a keywords entity (This only applies to keywords entities.)
    /// </summary>
    /// <param name="body">{"keyword": "Brussels", "synonyms": ["Brussels", "Capital of Europe"]}</param>
    /// <param name="entity">name of entity</param>
    /// <returns>json with id, name, roles, lookups and keywords with synonyms</returns>
    public async Task<string> CallPostEntityAsync(string body, string entity)
    {
      string url = $"{_urlEntity}/{entity}/keywords?v={_version}";
      return await PostOrDeleteAsync(url, body);
    }

    /// <summary>
    /// Create a new entity
    /// </summary>
    /// <param name="body">{"name":"favorite_city","roles":[]}</param>
    /// <returns>json with id, roles, lookups and keywords</returns>
    public async Task<string> CallPostEntityAsync(string body)
    {
      string url = $"{_urlEntity}?v={_version}";
      return await PostOrDeleteAsync(url, body);
    }
    #endregion

    public async Task<string> CallDeleteEntityAsync(string entity, string keyword, string synonym)
    {
      string url = $"{_urlEntity}/{entity}/keywords/{keyword}/synonyms/{synonym}?v={_version}";
      return await PostOrDeleteAsync(url);
    }
    public async Task<string> CallDeleteEntityAsync(string entity, string keyword)
    {
      string url = $"{_urlEntity}/{entity}/keywords/{keyword}?v={_version}";
      return await PostOrDeleteAsync(url);
    }
    public async Task<string> CallDeleteEntityAsync(string entity)
    {
      string url = $"{_urlEntity}/{entity}?v={_version}";
      return await PostOrDeleteAsync(url);
    }

    #region Private Functions

    private async Task<string> PostOrDeleteAsync(string url, string body = "")
    {
      var msg = "error";
      RestResponse response;
      try {
        if (body == "")
          response = await CallPostOrDeleteAsync(url);
        else
          response = await CallPostOrDeleteAsync(url, body);
        if (response != null && response.Content != null && response.IsSuccessful)
          msg = response.Content;
      }
      catch (Exception ex) {
        throw new Exception(ex.Message);
      }
      return msg;
    }

    private async Task<RestResponse> CallPostOrDeleteAsync(string url, string body = "")
    {
      var client = new RestClient(url);
      var request = new RestRequest();
      request.Method = body == "" ? RestSharp.Method.Delete : RestSharp.Method.Post;
      request.AddHeader("Authorization", _secureKey);
      request.AddHeader("Content-Type", "application/json");
      if (body != "")
        request.AddParameter("application/json", body, ParameterType.RequestBody);
      return await client.ExecuteAsync(request);
    }

    #endregion
  }
}

