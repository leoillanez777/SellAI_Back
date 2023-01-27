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

namespace SellAI.Controllers
{
  [Authorize]
  [Route("api/[controller]")]
  [ApiController]
  public class InterpreterController : ControllerBase
  {
    private readonly IInterpreter _db;
    public InterpreterController(IInterpreter interpreter)
    {
      _db = interpreter;
    }

    [HttpGet("{message}")]
    public async Task<IActionResult> SendMessage(string message)
    {
      var identity = HttpContext.User.Identity as ClaimsIdentity;
      if (identity != null) {
        await _db.SendMessageAsync(message, identity);
        return StatusCode(StatusCodes.Status200OK);
      }
      return StatusCode(StatusCodes.Status401Unauthorized);
    }
  }
}
