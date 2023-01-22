using System;
namespace SellAI.Models.DTOs
{
  public class SignInDTO
  {
    public string UserName { get; set; } = null!;
    public string DisplayName { get; set; } = null!;
    public string MenuJson { get; set; } = null!;
    public string Token { get; set; } = null!;
    public string Deposit { get; set; } = null!;
  }
}

