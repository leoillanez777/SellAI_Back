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
        private readonly string urlCategory = "https://api.wit.ai/entities/product_category/keywords";
        private readonly string urlBrand = "https://api.wit.ai/entities/product_brand/keywords";

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

        public async Task<string> CallPostCategoryAsync(string body)
        {
            
            var msg = "error";
            try 
            {
                var client = new RestClient(urlCategory + "?v=20221114");
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

        public async Task<string> CallDeleteCategoryAsync(string keyword)
        {
            var msg = "error";
            try
            {
                var getUrlEliminar = urlCategory + "/" + keyword + "?v=20221114";
                var client = new RestClient(getUrlEliminar);
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

        public async Task<string> CallPostBrandAsync(string body)
        {
            var msg = "error";
            try
            {
                var client = new RestClient(urlBrand + "?v=20221114");
                var request = new RestRequest();
                request.Method = RestSharp.Method.Post;
                request.AddHeader("Authorization", _secureKey);
                request.AddHeader("Content-Type", "application/json");
                request.AddParameter("application/json", body, ParameterType.RequestBody);
                RestResponse response = client.Execute(request);
                if (response != null && response.Content != null && response.IsSuccessful)
                    msg = "ok";
            }
            catch (Exception e)
            {

            }
            return msg;
        }

        public async Task<string> CallDeleteBrandAsync(string keyword)
        {
            var msg = "error";
            try
            {
                var getUrlEliminar = urlBrand + "/" + keyword + "?v=20221114";
                var client = new RestClient(getUrlEliminar);
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

