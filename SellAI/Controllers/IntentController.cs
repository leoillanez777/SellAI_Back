using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SellAI.Interfaces;
using SellAI.Models.AI.Objects;

namespace SellAI.Controllers;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class IntentController : ControllerBase {

  private readonly IIntent _intent;
  public IntentController(IIntent intent)
  {
    _intent = intent;
  }

  [HttpGet]
  public async Task<IActionResult> GetIntents()
  {
    string response = await _intent.GetAllIntent();
    return Ok(new { data = response });
  }

  [HttpGet("{id}")]
  public async Task<IActionResult> GetIntent(string id)
  {
    string response = await _intent.GetAllIntent();
    return Ok(new { data = response });
  }
}

