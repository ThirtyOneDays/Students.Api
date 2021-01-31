using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Students.Repository.Entities
{
  public class DbGroup
  {
    public long Id { get; set; }

    [MaxLength(25)]
    public string Name { get; set; }


    public List<DbStudent> Student { get; set; } = new List<DbStudent>();
  }
}
