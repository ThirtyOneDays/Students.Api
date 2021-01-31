namespace Students.Api.Infrastructure.Security.Auth
{
  public class JwtAuthResult
  {
    public string AccessToken { get; set; }
    public RefreshToken RefreshToken { get; set; }
  }
}
