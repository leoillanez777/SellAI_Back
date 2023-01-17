using System;
using SellAI.Models.AI;

namespace SellAI.Interfaces
{
  public interface IRestApi
  {
    Task<Message> MessageAsync(string message);
  }
}

