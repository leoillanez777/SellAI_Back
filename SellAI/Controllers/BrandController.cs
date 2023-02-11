using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SellAI.Interfaces;
using SellAI.Models;
using SellAI.Models.DTOs;
using System.Drawing.Drawing2D;

namespace SellAI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class BrandController : ControllerBase
    {
        private readonly IBrand _db;
        public BrandController(IBrand brand)
        {
            _db = brand;
        }


        [HttpGet]
        public async Task<IActionResult> GetListAsync(string app = "", bool isActive = true)
        {
            var data = await _db.GetListAsync(app, isActive);
            return StatusCode(StatusCodes.Status200OK, data);
        }

        [HttpPost]
        public async Task<IActionResult> PostAsync(BrandDTO brand)
        {
            var data = await _db.PostAsync(brand);
            return StatusCode(StatusCodes.Status200OK, data);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateAsync(BrandDTO brand, string id)
        {
            var data = await _db.UpdateAsync(brand, id);
            return StatusCode(StatusCodes.Status200OK, data);
        }
    }
}
