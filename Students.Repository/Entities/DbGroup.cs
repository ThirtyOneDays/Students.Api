using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Students.Repository.Entities
{
  public class DbGroup
  {
    public int Id { get; set; }

    [MaxLength(25)]
    public string Name { get; set; }


    public List<DbStudent> Students { get; set; } = new List<DbStudent>();
  }
}
