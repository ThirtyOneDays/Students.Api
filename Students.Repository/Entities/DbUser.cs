using System.ComponentModel.DataAnnotations;

namespace Students.Repository.Entities
{
  public class DbUser
  {
    public long Id { get; set; }
    [MaxLength(64)]
    public string UserName { get; set; }
    [MaxLength(128)]
    public string Password { get; set; }
  }
}
