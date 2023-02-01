using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SellAI.Interfaces;
using SellAI.Models.AI;
using SellAI.Models.DTOs;

namespace SellAI.Controllers
{
  [Authorize]
  [Route("api/[controller]")]
  [ApiController]
  public class InterpreterController : ControllerBase
  {
    private readonly IInterpreter _db;
    private readonly IClaim _claim;

    public InterpreterController(IInterpreter interpreter, IClaim claim)
    {
      _db = interpreter;
      _claim = claim;
    }

    [HttpGet("{message}")]
    public async Task<IActionResult> SendMessage(string message)
    {
      string jsonResponse = "";
      var identity = HttpContext.User.Identity as ClaimsIdentity;
      if (identity != null) {

        // Get App & Roles from user.
        RoleAppDTO rolesApp = _claim.GetRoleAndApp(identity);

        // Call Service
        jsonResponse = await _db.SendMessageAsync(message, rolesApp);
        return StatusCode(StatusCodes.Status200OK, new { resp = jsonResponse });
      }
      return StatusCode(StatusCodes.Status401Unauthorized);
    }

    [HttpGet("{message}/{token}")]
    public async Task<IActionResult> SendMessage(string message, string token)
    {
      string jsonResponse = "";
      var identity = HttpContext.User.Identity as ClaimsIdentity;
      if (identity != null) {

        // Get App & Roles from user.
        RoleAppDTO rolesApp = _claim.GetRoleAndApp(identity);

        // Call Service
        jsonResponse = await _db.SendResponseAsync(message, token, rolesApp.App);
        return StatusCode(StatusCodes.Status200OK, new { resp = jsonResponse });
      }
      return StatusCode(StatusCodes.Status401Unauthorized);
    }
  }
}
