using System;
using SellAI.Interfaces;

namespace SellAI.Services;

public class IntentService : IIntent {

  private readonly IRestApi _restApi;

  public IntentService(IRestApi restApi)
  {
    _restApi = restApi;
  }

  public async Task<string> GetAllIntent(string? intent = null)
  {
    return await _restApi.CallGetIntetAsync(intent);
  }
}

