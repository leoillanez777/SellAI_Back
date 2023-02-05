using System;
using Microsoft.DotNet.Scaffolding.Shared.CodeModifier.CodeChange;
using Newtonsoft.Json;
using Org.BouncyCastle.Asn1.Crmf;
using RestSharp;
using SellAI.Interfaces;
using SellAI.Models.AI;
using SellAI.Models.AI.Objects;

namespace SellAI.Services
{
  public class RestApiService : IRestApi
  {
    private readonly string _secureKey;

    public RestApiService(IConfiguration configuration)
    {
      _secureKey = configuration.GetSection("Security").GetValue<string>("Wit.ai")!;
    }

    public async Task<Message> MessageAsync(string message)
    {
      Message msg = new();
      try {
        string v = DateTime.Now.ToString("yyyyMMdd");
        var client = new RestClient($"https://api.wit.ai/message?v=20230113&q={message}");
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

        public async Task<string> CallPostAsync(string body, RestClient client)
        {
            var msg = "error";
            try 
            {
                var request = new RestRequest();
                request.Method = RestSharp.Method.Post;
                request.AddHeader("Authorization", _secureKey);
                request.AddHeader("Content-Type", "application/json");
                request.AddParameter("application/json", body, ParameterType.RequestBody);
                RestResponse response = client.Execute(request);
                if (response != null && response.Content != null && response.IsSuccessful)
                    msg = "ok";
            }
            catch(Exception e)
            {

            }
            return msg;
        }

        public async Task<string> CallDeleteAsync(RestClient client)
        {
            var msg = "error";
            try
            {
                var request = new RestRequest();
                request.Method = RestSharp.Method.Delete;
                request.AddHeader("Authorization", _secureKey);
                request.AddHeader("Content-Type", "application/json");
                RestResponse response = client.Execute(request);
                if (response != null && response.Content != null && response.IsSuccessful)
                    msg = "ok";
            }
            catch (Exception e)
            {

            }
            return msg;
        }
    }
}

