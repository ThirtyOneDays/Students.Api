using System.Collections.Generic;
using System.Threading.Tasks;
using Students.Common.Models.UI;
using Students.Logic.Models.Students;

namespace Students.Logic.Services.Students
{
  public interface IStudentsService
  {
    Task CreateStudentAsync(CreateStudentRequest student);
    Task DeleteStudentAsync(long studentId);
    Task UpdateStudentAsync(UpdateStudentRequest student);
    Task<StudentModel> GetStudentAsync(long studentId);
    Task<List<StudentModel>> GetStudentsAsync(PagingModel pagingModel);
    Task AddStudentToGroupAsync(StudentGroupModel studentGroupModel);
    Task RemoveStudentFromGroupAsync(StudentGroupModel studentGroupModel);
  }
}
