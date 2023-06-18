using System;
using SellAI.Interfaces;

namespace SellAI.Services;

public class EntityService : IEntity {

  private readonly IRestApi _restApi;

  public EntityService(IRestApi restApi)
  {
    _restApi = restApi;
  }

  public async Task<string> GetAllEntities(string? entity = null)
  {
    return await _restApi.CallGetEntityAsync(entity);
  }
}

