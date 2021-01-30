using System.Collections.Generic;
using System.Threading.Tasks;
using Students.Common.Models.UI;
using Students.Logic.Models.Students;

namespace Students.Logic.Services.Students
{
  public interface IStudentsService
  {
    Task CreateStudent(Student student);
    Task DeleteStudent(long studentId);
    Task UpdateStudent(Student student);
    Task<List<Student>> GetStudents(PagingModel pagingModel);
  }
}
