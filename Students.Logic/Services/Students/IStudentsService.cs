using System.Collections.Generic;
using System.Threading.Tasks;
using Students.Common.Models.UI;
using Students.Logic.Models.Students;

namespace Students.Logic.Services.Students
{
  public interface IStudentsService
  {
    Task CreateStudent(CreateStudentRequest student);
    Task DeleteStudent(long studentId);
    Task UpdateStudent(UpdateStudentRequest student);
    Task<StudentModel> GetStudent(long studentId);
    Task<List<StudentModel>> GetStudents(PagingModel pagingModel);
  }
}
