using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SellAI.Interfaces;
using SellAI.Models.DTOs;
using SellAI.Services;

namespace SellAI.Controllers
{
  [AllowAnonymous]
  [Route("api/[controller]")]
  [ApiController]
  public class AuthenticationController : ControllerBase
  {
    private readonly IAuthentication _db;
    public AuthenticationController(IAuthentication authenticationService)
    {
      _db = authenticationService;
    }

    [HttpPost]
    public async Task<IActionResult> ValidToken(LoginDTO loginDTO)
    {
      string token = await _db.ValidAsync(loginDTO.User, loginDTO.Password);
      return Ok(new { token });
    }

    [HttpPost("Login")]
    public async Task<IActionResult> Login(LoginDTO loginDTO)
    {
      SignInDTO signIn = await _db.LoginAsync(loginDTO);
      return Ok(new { signIn });
    }
  }
}
