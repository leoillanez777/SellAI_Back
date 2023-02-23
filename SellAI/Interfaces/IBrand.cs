using SellAI.Models;
using SellAI.Models.DTOs;

namespace SellAI.Interfaces {
  public interface IBrand {

    /// <summary>
    /// Filter the list of brands from the application and if it considers its status
    /// </summary>
    /// <param name="claims">claims of authorize</param>
    /// <param name="isActive">filter active</param>
    /// <returns>List of brands accordin the filters</returns>
    Task<List<BrandDTO>> GetListAsync(RoleAppDTO claims, bool isActive);

    /// <summary>
    /// Save new brand to db and wit.ai
    /// </summary>
    /// <param name="brandDTO">Data to save</param>
    /// <param name="claims">claims of token</param>
    /// <returns>json with result or message of error</returns>
    /// <exception cref="Exception">Middleware controller</exception>
    Task<string> PostAsync(BrandDTO brandDTO, RoleAppDTO claims);

    /// <summary>
    /// update a brand in the colection's Brand, delete the old brand keyword and insert the new brand keyword
    /// </summary>
    /// <param name="brandDTO">Brand Object</param>
    /// <param name="claims">claims of token</param>
    /// <returns>Returns a message on the status of the operation in error or json it's ok</returns>
    Task<string> UpdateAsync(BrandDTO brandDTO, RoleAppDTO claims);

    /// <summary>
    /// Delete brand on DB but in wit.ai only if not exists in another app.
    /// </summary>
    /// <param name="id">Id of delete</param>
    /// <param name="claims">claims of token</param>
    /// <returns>indicates if was successful or not</returns>
    Task<string> DeleteAsync(string id, RoleAppDTO claims);
  }
}
