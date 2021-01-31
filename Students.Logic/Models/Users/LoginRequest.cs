using System.ComponentModel.DataAnnotations;

namespace Students.Logic.Models.Users
{
  public class LoginRequest
  {
    [Required]
    [StringLength(24, MinimumLength = 3)]
    public string UserName { get; set; }

    [Required]
    [StringLength(16, MinimumLength = 5)]
    public string Password { get; set; }
  }
}
