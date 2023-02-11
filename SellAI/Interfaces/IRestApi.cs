using System;
using RestSharp;
using SellAI.Models.AI;

namespace SellAI.Interfaces
{
  public interface IRestApi
  {
        Task<Message> MessageAsync(string message);
        Task<string> CallPostCategoryAsync(string body);
        Task<string> CallDeleteCategoryAsync(string keyword);
        Task<string> CallPostBrandAsync(string body);
        Task<string> CallDeleteBrandAsync(string keyword);
    }
}

