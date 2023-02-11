using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SellAI.Interfaces;
using SellAI.Models;
using SellAI.Models.DTOs;

namespace SellAI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategory _db;
        public CategoryController(ICategory category)
        {
            _db = category;
        }


        [HttpGet]
        public async Task<IActionResult> GetListAsync(string app = "", bool isActive = true)
        {
            var data = await _db.GetListAsync(app, isActive);
            return StatusCode(StatusCodes.Status200OK, data);
        }

        [HttpPost]
        public async Task<IActionResult> PostAsync(CategoryDTO category)
        {
            var data = await _db.PostAsync(category);
            return StatusCode(StatusCodes.Status200OK, data);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateAsync(CategoryDTO category, string id)
        {
            var data = await _db.UpdateAsync(category, id);
            return StatusCode(StatusCodes.Status200OK, data);
        }
    }
}
