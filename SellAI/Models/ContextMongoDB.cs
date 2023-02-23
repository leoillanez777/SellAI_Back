using System;
namespace SellAI.Models
{
  public class ContextMongoDB {
    #region B
    public string BrandCollectionName { get; set; } = null!;
    #endregion
    #region C
    public string CategoryCollectionName { get; set; } = null!;
    #endregion
    #region D
    public string DatabaseName { get; set; } = null!;
    public string DataCollectionName { get; set; } = null!;
    #endregion
    #region P
    public string ProductCollectionName { get; set; } = null!;
    #endregion
    #region S
    public string SysContextCollectionName { get; set; } = null!;
    public string SysLogCollectionName { get; set; } = null!;
    public string SysMenuCollectionName { get; set; } = null!;
    #endregion
    #region U
    public string UserCollectionName { get; set; } = null!;
    public string UserMenuCollectionName { get; set; } = null!;
    #endregion
  }
}

