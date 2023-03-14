using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SellAI.Interfaces;
using SellAI.Models;
using SellAI.Models.DTOs;

namespace SellAI.Controllers {
  [Authorize]
  [Route("api/[controller]")]
  [ApiController]
  public class CategoryController : ControllerBase {
    private readonly ICategory _db;
    private readonly IClaim _claim;

    public CategoryController(ICategory category, IClaim claim)
    {
      _db = category;
      _claim = claim;
    }

    [HttpGet]
    public async Task<IActionResult> GetListAsync(bool isActive = true)
    {
      RoleAppDTO rolesApp = _claim.GetRoleAndApp(HttpContext.User.Identity!);
      if (rolesApp != null) {
        var data = await _db.GetListAsync(rolesApp, isActive);
        return Ok(data);
      }

      return Unauthorized();
    }

    [HttpPost]
    public async Task<IActionResult> PostAsync(CategoryDTO category)
    {
      RoleAppDTO rolesApp = _claim.GetRoleAndApp(HttpContext.User.Identity!);
      if (rolesApp != null) {
        var data = await _db.PostAsync(category, rolesApp);
        if (data == "error")
          return BadRequest("La solicitud no se pudo procesar correctamente");
        return Created("Category", data);
      }

      return StatusCode(StatusCodes.Status401Unauthorized);
    }

    [HttpPut]
    public async Task<IActionResult> UpdateAsync(CategoryDTO category)
    {
      RoleAppDTO rolesApp = _claim.GetRoleAndApp(HttpContext.User.Identity!);
      if (rolesApp != null) {
        var data = await _db.UpdateAsync(category, rolesApp);
        if (data == "error")
          return BadRequest("La solicitud no se pudo procesar correctamente");
        return Ok(data);
      }

      return Unauthorized();
    }

    // TODO: Delete Services.
  }
}
