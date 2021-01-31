using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using AutoMapper.Internal;
using Microsoft.IdentityModel.Tokens;
using Students.Api.Settings;

namespace Students.Api.Infrastructure.Security.Auth
{
  public class JwtAuthManager : IJwtAuthManager
  {
    private readonly ConcurrentDictionary<string, RefreshToken> _usersRefreshTokens;
    private readonly JwtTokenConfig _jwtTokenConfig;
    private readonly byte[] _secret;

    public JwtAuthManager(JwtTokenConfig jwtTokenConfig)
    {
      _jwtTokenConfig = jwtTokenConfig;
      _usersRefreshTokens = new ConcurrentDictionary<string, RefreshToken>();
      _secret = Encoding.ASCII.GetBytes(jwtTokenConfig.Secret);
    }

    public void RemoveExpiredRefreshTokens(DateTime now)
    {
      var expiredTokens = _usersRefreshTokens.Where(x => x.Value.ExpireAt < now).ToList();
      expiredTokens.ForAll(t => _usersRefreshTokens.TryRemove(t.Key, out _));
    }

    public void RemoveRefreshTokenByUserId(long userId)
    {
      var refreshTokens = _usersRefreshTokens.Where(x => x.Value.UserId == userId).ToList();
      refreshTokens.ForAll(t => _usersRefreshTokens.TryRemove(t.Key, out _));
    }

    public JwtAuthResult GenerateTokens(long userId, string username, IList<Claim> claims, DateTime now)
    {
      var shouldAddAudienceClaim = string.IsNullOrWhiteSpace(claims?.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Aud)?.Value);
      var jwtToken = new JwtSecurityToken(
          _jwtTokenConfig.Issuer,
          shouldAddAudienceClaim ? _jwtTokenConfig.Audience : string.Empty,
          claims,
          expires: now.AddMinutes(_jwtTokenConfig.AccessTokenExpiration),
          signingCredentials: new SigningCredentials(new SymmetricSecurityKey(_secret), SecurityAlgorithms.HmacSha256Signature));
      var accessToken = new JwtSecurityTokenHandler().WriteToken(jwtToken);

      var refreshToken = new RefreshToken
      {
        UserId = userId,
        TokenString = GenerateRefreshTokenString(),
        ExpireAt = now.AddMinutes(_jwtTokenConfig.RefreshTokenExpiration)
      };
      _usersRefreshTokens.AddOrUpdate(refreshToken.TokenString, refreshToken, (s, t) => refreshToken);

      return new JwtAuthResult
      {
        AccessToken = accessToken,
        RefreshToken = refreshToken
      };
    }

    public IList<Claim> GenerateClaims(long userId, string username)
    {
      var claims = new[]
      {
        new Claim(CustomClaimTypes.UserId, userId.ToString()), 
        new Claim(ClaimTypes.Name, username)
      };

      return claims;
    }

    public JwtAuthResult Refresh(string refreshToken, string accessToken, DateTime now)
    {
      var (principal, jwtToken) = DecodeJwtToken(accessToken);
      if (jwtToken == null || !jwtToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256Signature))
      {
        throw new SecurityTokenException("Invalid token");
      }

      if (!_usersRefreshTokens.TryGetValue(refreshToken, out var existingRefreshToken))
      {
        throw new SecurityTokenException("Invalid token");
      }
      var userId = long.Parse(principal.Claims.First(c => c.Type == CustomClaimTypes.UserId).Value);
      if (existingRefreshToken.UserId != userId || existingRefreshToken.ExpireAt < now)
      {
        throw new SecurityTokenException("Invalid token");
      }

      var username = principal.Claims.First(c => c.Type == ClaimTypes.Name).Value;

      return GenerateTokens(userId, username, principal.Claims.ToList(), now);
    }

    public (ClaimsPrincipal, JwtSecurityToken) DecodeJwtToken(string token)
    {
      if (string.IsNullOrWhiteSpace(token))
      {
        throw new SecurityTokenException("Invalid token");
      }
      var principal = new JwtSecurityTokenHandler()
          .ValidateToken(token,
              new TokenValidationParameters
              {
                ValidateIssuer = true,
                ValidIssuer = _jwtTokenConfig.Issuer,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(_secret),
                ValidAudience = _jwtTokenConfig.Audience,
                ValidateAudience = true,
                ValidateLifetime = false,
                ClockSkew = TimeSpan.Zero
              },
              out var validatedToken);
      return (principal, validatedToken as JwtSecurityToken);
    }

    private static string GenerateRefreshTokenString()
    {
      var randomNumber = new byte[32];
      using var randomNumberGenerator = RandomNumberGenerator.Create();
      randomNumberGenerator.GetBytes(randomNumber);
      return Convert.ToBase64String(randomNumber);
    }
  }
}
