using SellAI.Models;
using SellAI.Models.DTOs;

namespace SellAI.Interfaces
{
  public interface ICategory {
    /// <summary>
    /// Filter the list of categories from the application and if it considers its status
    /// </summary>
    /// <param name="claims">claims of token</param>
    /// <param name="isActive">filter active</param>
    /// <returns>List of Categories accordin the filters</returns>
    Task<List<CategoryDTO>> GetListAsync(RoleAppDTO claims, bool isActive);

    /// <summary>
    /// Save new category to db and wit.ai
    /// </summary>
    /// <param name="categoryDTO">Data to save</param>
    /// <param name="claims">claims of token</param>
    /// <returns>json with result or message of error</returns>
    /// <exception cref="Exception">Middleware controller</exception>
    Task<string> PostAsync(CategoryDTO categoryDTO, RoleAppDTO claims);

    /// <summary>
    /// update a category in the collection's Category, delete the old category keyword and insert the new category keyword
    /// </summary>
    /// <param name="categoryDTO">Category Object</param>
    /// <param name="claims">claims of token</param>
    /// <returns>Returns a message on the status of the operation in error or json it's ok</returns>
    Task<string> UpdateAsync(CategoryDTO categoryDTO, RoleAppDTO claims);

    /// <summary>
    /// Delete category on DB but in wit.ai only if not exists in another app.
    /// </summary>
    /// <param name="id">Id of delete</param>
    /// <param name="claims">claims of token</param>
    /// <returns>indicates if was successful or not</returns>
    Task<string> DeleteAsync(string id, RoleAppDTO claims);
  }
}
