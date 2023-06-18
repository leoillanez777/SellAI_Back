using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SellAI.Interfaces;
using SellAI.Models.AI.Objects;

namespace SellAI.Controllers;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class EntityController : ControllerBase {

  private readonly IEntity _entity;
  public EntityController(IEntity entity)
  {
    _entity = entity;
  }

  [HttpGet]
  public async Task<IActionResult> GetEntities()
  {
    string response = await _entity.GetAllEntities();
    return Ok(new { data = response });
  }

  [HttpGet("{name}")]
  public async Task<IActionResult> GetEntities(string name)
  {
    string response = await _entity.GetAllEntities(name);
    return Ok(new { data = response });
  }
}

