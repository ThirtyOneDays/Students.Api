using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Students.Common.Models.UI;
using Students.Logic.Models.Students;
using Students.Logic.Services.Students;

namespace Students.Api.Controllers
{
  [Route("students")]
  [ApiController]
  public class StudentsController : ControllerBase
  {
    private readonly IStudentsService _studentsService;

    public StudentsController(IStudentsService studentsService)
    {
      _studentsService = studentsService;
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> CreateStudent(CreateStudentRequest student)
    {
      await _studentsService.CreateStudentAsync(student);
      return NoContent();
    }

    [HttpPut]
    [Authorize]
    public async Task<IActionResult> UpdateStudent(UpdateStudentRequest student)
    {
      await _studentsService.UpdateStudentAsync(student);
      return NoContent();
    }

    [HttpDelete]
    [Authorize]
    public async Task<IActionResult> DeleteStudent(long studentId)
    {
      await _studentsService.DeleteStudentAsync(studentId);
      return NoContent();
    }

    [HttpGet]
    [Authorize]
    [ProducesResponseType(typeof(StudentModel), 200)]
    public async Task<IActionResult> GetStudent(long studentId)
    {
      var student = await _studentsService.GetStudentAsync(studentId);
      return Ok(student);
    }

    [HttpPost("list")]
    [Authorize]
    [ProducesResponseType(typeof(List<StudentModel>), 200)]
    public async Task<IActionResult> GetStudents(PagingModel pagingModel)
    {
      var students = await _studentsService.GetStudentsAsync(pagingModel);
      return Ok(students);
    }

    [HttpPost("groups")]
    [Authorize]
    public async Task<IActionResult> AddStudentToGroup(StudentGroupModel studentGroupModel)
    {
      await _studentsService.AddStudentToGroupAsync(studentGroupModel);
      return NoContent();
    }

    [HttpDelete("groups")]
    //[Authorize]
    public async Task<IActionResult> RemoveStudentFromGroup(StudentGroupModel studentGroupModel)
    {
      await _studentsService.RemoveStudentFromGroupAsync(studentGroupModel);
      return NoContent();
    }
  }
}
