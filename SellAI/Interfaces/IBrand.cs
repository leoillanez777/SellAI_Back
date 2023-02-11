using SellAI.Models;
using SellAI.Models.DTOs;

namespace SellAI.Interfaces
{
    public interface IBrand
    {
        Task<List<Brand>> GetListAsync(string app, bool isActive);
        Task<string> PostAsync(BrandDTO category);
        Task<string> UpdateAsync(BrandDTO category, string id);
    }
}
