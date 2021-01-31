using System.ComponentModel.DataAnnotations;

namespace Students.Logic.Models.Students
{
  public class CreateStudentRequest
  {
    [Range(1, int.MaxValue)]
    [Required]
    public Gender Gender { get; set; }
    [Required]
    public string LastName { get; set; }
    [Required]
    public string FirstName { get; set; }
    public string Patronymic { get; set; }
    public string UId { get; set; }
  }
}
