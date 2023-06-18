using System;
namespace SellAI.Interfaces;

public interface IIntent {

  /// <summary>
  /// Get all intents 
  /// </summary>
  /// <returns>json string</returns>
  Task<string> GetAllIntent(string? intent = null);
}
