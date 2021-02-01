using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Net.Http.Headers;
using Students.Api.Infrastructure.Security.Auth;
using Students.Logic.Models.Users;
using Students.Logic.Services.Users;

namespace Students.Api.Controllers
{
  [Route("users")]
  [ApiController]
  public class UsersController : ControllerBase
  {
    private readonly IUsersService _usersService;
    private readonly IJwtAuthManager _jwtAuthManager;

    public UsersController(IUsersService usersService, IJwtAuthManager jwtAuthManager)
    {
      _usersService = usersService;
      _jwtAuthManager = jwtAuthManager;
    }

    [HttpPost("sign-up")]
    [AllowAnonymous]
    public async Task<IActionResult> SignUp(LoginRequest request)
    {
      var result = await _usersService.TryInsertIfUniqueAsync(request);
      if (result == null)
      {
        return Unauthorized("Username exists. Try another one");
      }

      var claims = _jwtAuthManager.GenerateClaims(result.UserId, result.UserName);
      var jwtResult = _jwtAuthManager.GenerateTokens(result.UserId, result.UserName, claims, DateTime.Now);
      result.AccessToken = jwtResult.AccessToken;
      result.RefreshToken = jwtResult.RefreshToken.TokenString;

      return Ok(result);
    }

    [AllowAnonymous]
    [HttpPost("sign-in")]
    public async Task<IActionResult> SignIn(LoginRequest request)
    {
      var result = await _usersService.GetUserAsync(request);
      if (result == null)
        return Unauthorized("Invalid arguments");

      var claims = _jwtAuthManager.GenerateClaims(result.UserId, result.UserName);
      var jwtResult = _jwtAuthManager.GenerateTokens(result.UserId, request.UserName, claims, DateTime.Now);
      result.AccessToken = jwtResult.AccessToken;
      result.RefreshToken = jwtResult.RefreshToken.TokenString;

      return Ok(result);
    }

    [HttpPost("sign-out")]
    [Authorize]
    public IActionResult Logout()
    {
      var userId = long.Parse(User.Claims.First(c => c.Type == CustomClaimTypes.UserId).Value);
      _jwtAuthManager.RemoveRefreshTokenByUserId(userId);

      return NoContent();
    }

    [HttpPost("refresh-token")]
    [AllowAnonymous]
    public IActionResult RefreshToken(RefreshTokenRequest request)
    {
      try
      {
        if (string.IsNullOrWhiteSpace(request.RefreshToken))
        {
          return Unauthorized();
        }

        var accessToken = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
        var jwtResult = _jwtAuthManager.Refresh(request.RefreshToken, accessToken, DateTime.Now);

        return Ok(new RefreshTokenResponse()
        {
          AccessToken = jwtResult.AccessToken,
          RefreshToken = jwtResult.RefreshToken.TokenString
        });
      }
      catch (SecurityTokenException e)
      {
        return Unauthorized(e.Message);
      }
    }
  }
}
