using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Students.Common.Models.UI;
using Students.Logic.Models.Students;
using Students.Repository.Entities;
using Students.Repository.Interfaces;

namespace Students.Logic.Services.Students
{
  public class StudentsService : IStudentsService
  {
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public StudentsService(IUnitOfWork unitOfWork, IMapper mapper)
    {
      _unitOfWork = unitOfWork;
      _mapper = mapper;
    }

    public async Task CreateStudent(Student student)
    {
      var dbStudent = _mapper.Map<DbStudent>(student);
      await _unitOfWork.StudentRepository.Create(dbStudent);
    }

    public Task DeleteStudent(long studentId)
    {
      throw new System.NotImplementedException();
    }

    public Task UpdateStudent(Student student)
    {
      throw new System.NotImplementedException();
    }

    public async Task<List<Student>> GetStudents(PagingModel pagingModel)
    {
      var dbStudents = await _unitOfWork.StudentRepository.GetAsync(pagingModel, includeProperties: "Groups");
      return _mapper.Map<List<Student>>(dbStudents);
    }
  }
}
