using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Students.Repository.Entities
{
  public class DbStudent
  {
    public int Id { get; set; }

    public int Gender { get; set; }

    [MaxLength(40)]
    public string LastName { get; set; }

    [MaxLength(40)]
    public string FirstName { get; set; }

    [MaxLength(60)]
    public string Patronymic { get; set; }

    [MinLength(6), MaxLength(16)]
    public string UId { get; set; }


    public List<DbGroup> Groups { get; set; } = new List<DbGroup>();
  }
}
