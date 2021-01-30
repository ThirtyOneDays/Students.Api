using System.Collections.Generic;
using System.Threading.Tasks;
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
    //[Authorize]
    [ProducesResponseType(typeof(Student), 200)]
    public async Task<IActionResult> CreateStudent(Student student)
    {
      await _studentsService.CreateStudent(student);
      return NoContent();
    }

    [HttpPost("get")]
    //[Authorize]
    [ProducesResponseType(typeof(List<Student>), 200)]
    public async Task<IActionResult> GetStudents(PagingModel pagingModel)
    {
      var students = await _studentsService.GetStudents(pagingModel);
      return Ok(students);
    }
  }
}
