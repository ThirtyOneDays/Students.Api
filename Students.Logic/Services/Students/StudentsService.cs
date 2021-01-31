using System.Collections.Generic;
using System.Linq;
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

    public async Task CreateStudentAsync(CreateStudentRequest student)
    {
      if (student == null)
        throw new InvalidArgumentException("Create student request model cannot be empty.");

      var dbStudent = _mapper.Map<DbStudent>(student);
      await _unitOfWork.StudentRepository.CreateAsync(dbStudent);
    }

    public async Task DeleteStudentAsync(long studentId)
    {
      var dbStudent = await _unitOfWork.StudentRepository.FindByIdAsync(studentId);
      if (dbStudent == null)
        throw new NotFoundException(typeof(DbStudent), studentId);

      await _unitOfWork.StudentRepository.RemoveAsync(dbStudent);
    }

    public async Task UpdateStudentAsync(UpdateStudentRequest student)
    {
      if (student == null)
        throw new InvalidArgumentException("Update student request model cannot be empty.");

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

    public async Task<StudentModel> GetStudentAsync(long studentId)
    {
      var dbStudent = await _unitOfWork.StudentRepository.FindByIdAsync(studentId);
      if (dbStudent == null)
        throw new NotFoundException(typeof(DbStudent), studentId);

      return _mapper.Map<StudentModel>(dbStudent);
    }

    public async Task<List<StudentModel>> GetStudentsAsync(PagingModel pagingModel)
    {
      var dbStudents = await _unitOfWork.StudentRepository.GetListAsync(pagingModel, includeProperties: nameof(DbStudent.Group));
      return _mapper.Map<List<StudentModel>>(dbStudents);
    }

    public async Task AddStudentToGroupAsync(StudentGroupModel studentGroupModel)
    {
      var dbStudent = await _unitOfWork.StudentRepository.GetAsync(x => x.Id == studentGroupModel.StudentId, includeProperties: nameof(DbStudent.Group));
      if (dbStudent == null)
        throw new NotFoundException(typeof(DbStudent), studentGroupModel.StudentId);

      var existingStudentGroup = dbStudent.Group.FirstOrDefault(x => x.Id == studentGroupModel.GroupId);
      if (existingStudentGroup != null)
        throw new InvalidArgumentException("Student is already in this group."); ;

      var dbGroup = await _unitOfWork.GroupRepository.FindByIdAsync(studentGroupModel.GroupId);
      if (dbGroup == null)
        throw new NotFoundException(typeof(DbGroup), studentGroupModel.GroupId);

      dbStudent.Group.Add(dbGroup);

      await _unitOfWork.StudentRepository.UpdateAsync(dbStudent);
    }

    public async Task RemoveStudentFromGroupAsync(StudentGroupModel studentGroupModel)
    {
      var dbStudent = await _unitOfWork.StudentRepository.GetAsync(x => x.Id == studentGroupModel.StudentId, includeProperties: nameof(DbStudent.Group));
      if (dbStudent == null)
        throw new NotFoundException(typeof(DbStudent), studentGroupModel.StudentId);

      dbStudent.Group.RemoveAll(x => x.Id == studentGroupModel.GroupId);

      await _unitOfWork.StudentRepository.UpdateAsync(dbStudent);
    }
  }
}
