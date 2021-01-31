using System.ComponentModel.DataAnnotations;

namespace Students.Logic.Models.Students
{
  public class StudentGroupModel
  {
    [Required]
    public long StudentId { get; set; }
    [Required]
    public long GroupId { get; set; }
  }
}
