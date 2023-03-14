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
      RoleAppDTO rolesApp = _claim.GetRoleAndApp(HttpContext.User.Identity!);
      if (rolesApp != null) {
        // Call Service
        jsonResponse = await _db.SendMessageAsync(message, rolesApp);
        return Ok(new { resp = jsonResponse });
      }
      return Unauthorized();
    }

    [HttpGet("{message}/{token}")]
    public async Task<IActionResult> SendMessage(string message, string token)
    {
      string jsonResponse = "";
      RoleAppDTO rolesApp = _claim.GetRoleAndApp(HttpContext.User.Identity!);
      if (rolesApp != null) {
        // Call Service
        jsonResponse = await _db.SendResponseAsync(message, token, rolesApp);
        return Ok(new { resp = jsonResponse });
      }
      return Unauthorized();
    }

    [HttpPost("audio")]
    public async Task<IActionResult> Speech([FromForm] IFormFile file)
    {
      if (file == null || file.Length == 0) {
        return BadRequest("Audio file not provided");
      }
      string jsonResponse = "";
      RoleAppDTO rolesApp = _claim.GetRoleAndApp(HttpContext.User.Identity!);
      if (rolesApp != null) {
        byte[] bytes;
        using (var memoryStream = new MemoryStream()) {
          file.CopyTo(memoryStream);
          bytes = memoryStream.ToArray();
        }
        jsonResponse = await _db.SpeechAsync(bytes, rolesApp);
        return Ok(new { resp = jsonResponse });
      }
      return Unauthorized();
    }
  }
}
