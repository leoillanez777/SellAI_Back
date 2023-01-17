using System;
namespace SellAI.Models
{
  public class ContextMongoDB
  {
    public string DatabaseName { get; set; } = null!;
    public string UserCollectionName { get; set; } = null!;
    public string SysMenuCollectionName { get; set; } = null!;
  }
}

