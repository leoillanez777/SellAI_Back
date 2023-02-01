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
  }
}

