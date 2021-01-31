using System;
using System.Collections.Generic;
using System.Text;

namespace Students.Logic.Models.Users
{
  public class LoginResponse
  {
    public long UserId { get; set; }
    public string UserName { get; set; }
    public string AccessToken { get; set; }
    public string RefreshToken { get; set; }
  }
}
