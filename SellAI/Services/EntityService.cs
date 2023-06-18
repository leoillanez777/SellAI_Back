using System;
using SellAI.Interfaces;

namespace SellAI.Services;

public class IntentService : IIntent {

  private readonly IRestApi _restApi;

  public IntentService(IRestApi restApi)
  {
    _restApi = restApi;
  }

  public async Task<string> GetAllIntent()
  {
    return await _restApi.CallGetIntetAsync();
  }
}

