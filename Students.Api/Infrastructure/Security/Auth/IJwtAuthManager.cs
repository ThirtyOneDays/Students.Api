using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Students.Api.Infrastructure.Security.Auth
{
  public interface IJwtAuthManager
  {
    JwtAuthResult GenerateTokens(long userId, string username, IList<Claim> claims, DateTime now);
    JwtAuthResult Refresh(string refreshToken, string accessToken, DateTime now);
    void RemoveExpiredRefreshTokens(DateTime now);
    void RemoveRefreshTokenByUserId(long userId);
    IList<Claim> GenerateClaims(long userId, string username);
    (ClaimsPrincipal, JwtSecurityToken) DecodeJwtToken(string token);
  }
}
