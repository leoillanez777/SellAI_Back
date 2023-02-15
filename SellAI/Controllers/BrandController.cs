using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SellAI.Interfaces;
using SellAI.Models;
using SellAI.Models.DTOs;
using System.Drawing.Drawing2D;
using System.Security.Claims;

namespace SellAI.Controllers {
  [Authorize]
  [Route("api/[controller]")]
  [ApiController]
  public class BrandController : ControllerBase {

    private readonly IBrand _db;
    private readonly IClaim _claim;

    public BrandController(IBrand brand, IClaim claim)
    {
      _db = brand;
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
    public async Task<IActionResult> PostAsync(BrandDTO brand)
    {
      var identity = HttpContext.User.Identity as ClaimsIdentity;
      if (identity != null) {
        // Get App & Roles from user.
        RoleAppDTO rolesApp = _claim.GetRoleAndApp(identity);

        var data = await _db.PostAsync(brand, rolesApp.App);
        return StatusCode(StatusCodes.Status201Created, data);
      }
      return StatusCode(StatusCodes.Status401Unauthorized);
    }

    [HttpPut]
    public async Task<IActionResult> UpdateAsync(BrandDTO brand, string id)
    {
      var identity = HttpContext.User.Identity as ClaimsIdentity;
      if (identity != null) {
        // Get App & Roles from user.
        RoleAppDTO rolesApp = _claim.GetRoleAndApp(identity);

        var data = await _db.UpdateAsync(brand, id, rolesApp.App);
        return StatusCode(StatusCodes.Status202Accepted, data);
      }
      return StatusCode(StatusCodes.Status401Unauthorized);
    }
  }
}
