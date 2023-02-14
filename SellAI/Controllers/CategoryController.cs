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
      var identity = HttpContext.User.Identity as ClaimsIdentity;
      if (identity != null) {
        // Get App & Roles from user.
        RoleAppDTO rolesApp = _claim.GetRoleAndApp(identity);

        var data = await _db.GetListAsync(rolesApp.App, isActive);
        return StatusCode(StatusCodes.Status200OK, data);
      }
      return StatusCode(StatusCodes.Status401Unauthorized);
    }

    [HttpPost]
    public async Task<IActionResult> PostAsync(CategoryDTO category)
    {
      var identity = HttpContext.User.Identity as ClaimsIdentity;
      if (identity != null) {
        // Get App & Roles from user.
        RoleAppDTO rolesApp = _claim.GetRoleAndApp(identity);

        var data = await _db.PostAsync(category, rolesApp.App);
        return StatusCode(StatusCodes.Status201Created, data);
      }
      return StatusCode(StatusCodes.Status401Unauthorized);
    }

    [HttpPut]
    public async Task<IActionResult> UpdateAsync(CategoryDTO category, string id)
    {
      var identity = HttpContext.User.Identity as ClaimsIdentity;
      if (identity != null) {
        // Get App & Roles from user.
        RoleAppDTO rolesApp = _claim.GetRoleAndApp(identity);

        var data = await _db.UpdateAsync(category, id, rolesApp.App);
        return StatusCode(StatusCodes.Status202Accepted, data);
      }
      return StatusCode(StatusCodes.Status401Unauthorized);
    }
  }
}
