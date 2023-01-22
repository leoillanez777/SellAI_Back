using System;
using SellAI.Models;
using SellAI.Models.Objects;

namespace SellAI.Interfaces
{
  public interface IUserMenu
  {
    Task<List<User_Menu>> GetMenuAsync(List<string> roles);
  }
}

