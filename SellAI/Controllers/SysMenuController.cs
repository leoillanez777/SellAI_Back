using System;
using Microsoft.AspNetCore.Mvc;
using SellAI.Interfaces;
using SellAI.Models.DTOs;
using SellAI.Models.Objects;

namespace SellAI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SysMenuController: ControllerBase {

  private readonly IClaim _claim;
  private readonly ISysMenu _sysmenu;

  public SysMenuController(ISysMenu sysMenu, IClaim claim)
  {
    _sysmenu = sysMenu;
    _claim = claim;
  }

  [HttpGet]
  public async Task<IActionResult> GetIntentName(string name)
  {
    RoleAppDTO rolesApp = _claim.GetRoleAndApp(HttpContext.User.Identity!);
    if (rolesApp != null) {
      var data = await _sysmenu.GetIntentAsync(name, rolesApp);
      return Ok(data);
    }

    return Unauthorized();
  }

  [HttpGet("{id}")]
  public async Task<IActionResult> GetIntentID(string id)
  {
    RoleAppDTO rolesApp = _claim.GetRoleAndApp(HttpContext.User.Identity!);
    if (rolesApp != null) {
      var data = await _sysmenu.GetAsync(id, rolesApp);
      return Ok(data);
    }

    return Unauthorized();
  }
}

