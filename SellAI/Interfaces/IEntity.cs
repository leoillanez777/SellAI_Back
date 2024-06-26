﻿using System;
namespace SellAI.Interfaces;

public interface IEntity {

  /// <summary>
  /// Get all entities 
  /// </summary>
  /// <returns>json string</returns>
  Task<string> GetAllEntities(string? entity = null);
}
