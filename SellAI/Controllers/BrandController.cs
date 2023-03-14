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
      RoleAppDTO rolesApp = _claim.GetRoleAndApp(HttpContext.User.Identity!);
      if (rolesApp != null) {
        var data = await _db.GetListAsync(rolesApp, isActive);
        return Ok(data);
      }

      return Unauthorized();
    }

    [HttpPost]
    public async Task<IActionResult> PostAsync(BrandDTO brand)
    {
      RoleAppDTO rolesApp = _claim.GetRoleAndApp(HttpContext.User.Identity!);
      if (rolesApp != null) {
        var data = await _db.PostAsync(brand, rolesApp);
        if (data == "error")
          return BadRequest("La solicitud no se pudo procesar correctamente.");
        return Created("Brand", data);
      }

      return Unauthorized();
    }

    [HttpPut]
    public async Task<IActionResult> UpdateAsync(BrandDTO brand)
    {
      RoleAppDTO rolesApp = _claim.GetRoleAndApp(HttpContext.User.Identity!);
      if (rolesApp != null) {
        var data = await _db.UpdateAsync(brand, rolesApp);
        if (data == "error")
          return BadRequest("La solicitud no se pudo procesar correctamente.");
        return Ok(data);
      }

      return Unauthorized();
    }

    //TODO: add delete request.
  }
}
