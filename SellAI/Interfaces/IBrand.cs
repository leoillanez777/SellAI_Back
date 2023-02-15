using SellAI.Models;
using SellAI.Models.DTOs;

namespace SellAI.Interfaces {
  public interface IBrand {

    /// <summary>
    /// Filter the list of Categories from the application and if it considers its status
    /// </summary>
    /// <param name="app">Name of aplication</param>
    /// <param name="isActive">filter active</param>
    /// <returns>List of Categories accordin the filters</returns>
    Task<List<Brand>> GetListAsync(string app, bool isActive);

    /// <summary>
    /// Save new brand to db and wit.ai
    /// </summary>
    /// <param name="brandDTO">Data to save</param>
    /// <param name="app">name of app</param>
    /// <returns>json with result or message of error</returns>
    /// <exception cref="Exception">Middleware controller</exception>
    Task<string> PostAsync(BrandDTO category, string app);

    /// <summary>
    /// update a category in the colection's Category, delete the old category keyword and insert the new category keyword
    /// </summary>
    /// <param name="brandDTO">Brand Object</param>
    /// <param name="id">Id Object</param>
    /// <param name="app">Name of app</param>
    /// <returns>Returns a message on the status of the operation in error or json it's ok</returns>
    Task<string> UpdateAsync(BrandDTO category, string id, string app);
  }
}
