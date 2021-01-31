using System;

namespace Students.Api.Infrastructure.Security.Auth
{
  public class RefreshToken
  {
    public long UserId { get; set; } 
    public string TokenString { get; set; }
    public DateTime ExpireAt { get; set; }
  }
}
