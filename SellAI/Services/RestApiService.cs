using System;
using System.Net;
using Newtonsoft.Json;
using RestSharp;
using RestSharp.Authenticators;
using SellAI.Interfaces;
using SellAI.Models.AI;
using SellAI.Models.Data;

namespace SellAI.Services
{
  public class RestApiService : IRestApi {
    private readonly string _secureKey;
    private readonly string _urlMsg;
    private readonly string _urlSpeech;
    private readonly string _urlEntity;
    private readonly string _urlIntent;
    private readonly string _version = DateTime.UtcNow.ToString("yyyyMMdd");

    public RestApiService(IConfiguration configuration)
    {
      var wit = configuration.GetSection("Wit.ai");
      _secureKey = wit.GetValue<string>("Security")!;
      _urlMsg = wit.GetValue<string>("UrlMessage")!;
      _urlSpeech = wit.GetValue<string>("UrlSpeech")!;
      _urlEntity = wit.GetValue<string>("UrlEntity")!;
      _urlIntent = wit.GetValue<string>("UrlIntent")!;
    }

    /// <summary>
    /// Get intent and entities from message
    /// </summary>
    /// <param name="message">text to send</param>
    /// <returns>Model Message</returns>
    public async Task<Message> MessageAsync(string message)
    {
      Message msg = new();
      RestResponse response = null!;
      try {
        var options = new RestClientOptions($"{_urlMsg}?v={_version}&q={message}") {
          ThrowOnAnyError = true,
          MaxTimeout = 10000,
          Authenticator = new JwtAuthenticator(_secureKey)
        };
        var client = new RestClient(options);
        var request = new RestRequest();
        //request.AddHeader("Authorization", _secureKey);
        response = await client.GetAsync(request);

        if (response != null && response.Content != null) {
          if (response.StatusCode == HttpStatusCode.OK) {
            msg = JsonConvert.DeserializeObject<Message>(response.Content)!;
          }
          else {
            var errorMessage = response.ErrorException != null ? response.ErrorException.Message : response.ErrorMessage;
            throw new Exception($"Se produjo un error al llamar a la API de mensajes. Código de estado HTTP: {(int)response.StatusCode}. Detalles: {errorMessage}");
          }
        }
      }
      catch (JsonException ex) {
        throw new Exception($"Se produjo un error al deserializar la respuesta de la API de mensajes: {ex.Message}. Detalles: {ex.StackTrace}");
      }
      catch (WebException ex) {
        // ATTENTION: see var response...now!
        throw new WebException($"Se produjo un error inesperado: {ex.Message}. Detalles: {ex.StackTrace}");
      }

      return msg;
    }

    public async Task<Message> SpeechAsync(byte[] audio)
    {
      Message msg = new();
      RestResponse response = null!;
      try {
        var options = new RestClientOptions($"{_urlSpeech}?v={_version}&n=1") {
          ThrowOnAnyError = true,
          MaxTimeout = 10000,
          Authenticator = new JwtAuthenticator(_secureKey)
        };
        var client = new RestClient(options);
        var request = new RestRequest { Method = RestSharp.Method.Post };
        request.AddHeader("Content-Type", "audio/mpeg");
        request.AddParameter("audio/mpeg", audio, ParameterType.RequestBody);

        response = await client.ExecuteAsync(request);

        if (response != null && response.Content != null) {
          if (response.StatusCode == HttpStatusCode.OK) {
            // Adding commas to the response because I received without them.
            string json = "[" + AddCommas(response.Content) + "]";
            var msgAudio = JsonConvert.DeserializeObject<List<Message>>(json)!;
            var finalMsgAudio = msgAudio.FirstOrDefault(m => m.EsFinal.HasValue && m.EsFinal.Value);
            if (finalMsgAudio != null && finalMsgAudio is Message) {
              msg = finalMsgAudio as Message;
            }
            else {
              throw new Exception("El audio no es entendible o no se reconoce lo que se intenta comunicar.");
            }
          }
          else {
            var errorMessage = response.ErrorException != null ? response.ErrorException.Message : response.ErrorMessage;
            throw new Exception($"Se produjo un error al llamar a la API de audio. Código de estado HTTP: {(int)response.StatusCode}. Detalles: {errorMessage}");
          }
        }
      }
      catch (JsonException ex) {
        throw new Exception($"Se produjo un error al deserializar la respuesta de la API de audio: {ex.Message}. Detalles: {ex.StackTrace}");
      }
      catch (HttpRequestException ex) {
        throw new HttpRequestException($"Se produjo un error inesperado: {ex.Message}. Detalles: {ex.StackTrace}");
      }
      catch (Exception ex) {
        throw new Exception($"Se produjo un error en la API de audio: {ex.Message}");
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

    #region Call Delete Entity

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

    #endregion

    #region Call Get Intents
    public async Task<string> CallGetIntetAsync(string? intent = null)
    {
      string url = $"{_urlIntent}{(intent is not null ? $"/{intent}" : "")}?v={_version}";
      return await GetAsync(url);
    }
    #endregion

    #region Call Get Entities
    public async Task<string> CallGetEntityAsync(string? entity = null)
    {
      string url = $"{_urlEntity}{(entity is not null ? $"/{entity}" : "")}?v={_version}";
      return await GetAsync(url);
    }
    #endregion

    #region Private Functions

    private async Task<string> GetAsync(string url)
    {
      var msg = "error";
      RestResponse response;
      try {
        var options = new RestClientOptions(url) {
          ThrowOnAnyError = true,
          MaxTimeout = 10000,
          Authenticator = new JwtAuthenticator(_secureKey)
        };

        var client = new RestClient(options);
        var request = new RestRequest();
        request.Method = RestSharp.Method.Get;
        request.AddHeader("Content-Type", "application/json");
        response = await client.ExecuteAsync(request);
        if (response != null && response.Content != null && response.IsSuccessful) {
          msg = response.Content;
        }
      }
      catch (Exception ex) {
        throw new Exception($"Se produjo un error inesperado en get Method: {ex.Message}. Detalles: {ex.StackTrace}");
      }
      return msg;
    }

    private async Task<string> PostOrDeleteAsync(string url, string body = "")
    {
      var msg = "error";
      RestResponse response;
      try {
        if (body == "")
          response = await CallPostOrDeleteAsync(url);
        else
          response = await CallPostOrDeleteAsync(url, body);
        if (response != null && response.Content != null && response.IsSuccessful) {
          if (response.ContentType == "application/json" && response.StatusCode == System.Net.HttpStatusCode.BadRequest) {
            ResponseError responseError = JsonConvert.DeserializeObject<ResponseError>(response.Content)!;
            if (responseError.error.Contains("already exists")) {
              msg = "already exists";// duplicated, perhaps because another created.
              // UNDONE: create synonyms? I don't know.
            }
            else {
              msg = responseError.error;
            }
          }
          else
            msg = response.Content;
        }
      }
      catch (Exception ex) {
        throw new Exception($"Se produjo un error inesperado: {ex.Message}. Detalles: {ex.StackTrace}");
      }
      return msg;
    }

    private async Task<RestResponse> CallPostOrDeleteAsync(string url, string body = "")
    {
      try {
        var options = new RestClientOptions(url) {
          ThrowOnAnyError = true,
          MaxTimeout = 10000,
          Authenticator = new JwtAuthenticator(_secureKey)
        };
        var client = new RestClient(options);
        var request = new RestRequest();
        request.Method = body == "" ? RestSharp.Method.Delete : RestSharp.Method.Post;
        request.AddHeader("Content-Type", "application/json");
        if (body != "")
          request.AddParameter("application/json", body, ParameterType.RequestBody);
        return await client.ExecuteAsync(request);
      }
      catch (Exception ex) {
        throw new Exception($"Se produjo un error inesperado: {ex.Message}. Detalles: {ex.StackTrace}");
      }
    }

    private string AddCommas(string input)
    {
      return input.Replace("\r\n{", ",{");
    }

    #endregion
  }
}

