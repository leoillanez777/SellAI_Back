using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SellAI.Interfaces;
using SellAI.Models.AI;

namespace SellAI.Controllers
{
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
      await _db.SendMessageAsync(message);
      return StatusCode(StatusCodes.Status200OK);
    }
  }
}
