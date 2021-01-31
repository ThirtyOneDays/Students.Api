using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Students.Common.Exceptions;
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

    public async Task CreateStudent(CreateStudentRequest student)
    {
      if (student == null)
        throw new InvalidArgumentException("Upsert student request model cannot be empty.");

      var dbStudent = _mapper.Map<DbStudent>(student);
      await _unitOfWork.StudentRepository.CreateAsync(dbStudent);
    }

    public async Task DeleteStudent(long studentId)
    {
      var dbStudent = await _unitOfWork.StudentRepository.FindByIdAsync(studentId);
      if (dbStudent == null)
        throw new NotFoundException(typeof(DbStudent), studentId);

      await _unitOfWork.StudentRepository.RemoveAsync(dbStudent);
    }

    public async Task UpdateStudent(UpdateStudentRequest student)
    {
      if (student == null)
        throw new InvalidArgumentException("Upsert student request model cannot be empty.");

      var dbStudent = _mapper.Map<DbStudent>(student);

      try
      {
        await _unitOfWork.StudentRepository.UpdateAsync(dbStudent);
      }
      catch (DbUpdateConcurrencyException)
      {
        throw new NotFoundException(typeof(DbStudent), student.Id);
      }
    }

    public async Task<StudentModel> GetStudent(long studentId)
    {
      var dbStudent = await _unitOfWork.StudentRepository.FindByIdAsync(studentId);
      if (dbStudent == null)
        throw new NotFoundException(typeof(DbStudent), studentId);

      return _mapper.Map<StudentModel>(dbStudent);
    }

    public async Task<List<StudentModel>> GetStudents(PagingModel pagingModel)
    {
      var dbStudents = await _unitOfWork.StudentRepository.GetAsync(pagingModel, includeProperties: nameof(DbStudent.Group));
      return _mapper.Map<List<StudentModel>>(dbStudents);
    }
  }
}
