﻿using SellAI.Models;
using SellAI.Models.DTOs;

namespace SellAI.Interfaces
{
  public interface ICategory {
    Task<List<Category>> GetListAsync(string app, bool isActive);
    Task<string> PostAsync(CategoryDTO category, string app);
    Task<string> UpdateAsync(CategoryDTO category, string id, string app);
  }
}
