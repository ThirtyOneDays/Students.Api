using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;
using AutoFixture;
using AutoFixture.AutoMoq;
using AutoMapper;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using Students.Logic.Models.Students;
using Students.Logic.Services.Students;
using Students.Repository.Entities;
using Students.Repository.Interfaces;

namespace Students.UnitTests.Students
{
  [TestFixture]
  public class StudentsServiceTests
  {
    private IFixture _fixture;
    private Mock<IUnitOfWork> _unitOfWorkMock;
    private IMapper _mapper;

    [SetUp]
    public void SetUp()
    {
      _fixture = new Fixture().Customize(new AutoMoqCustomization());

      _unitOfWorkMock = _fixture.Freeze<Mock<IUnitOfWork>>();

      var assemblies = new[]
      {
        Assembly.GetAssembly(typeof(StudentsMappingProfile)),
      };

      var config = new MapperConfiguration(cfg =>
      {
        cfg.AddMaps(assemblies);
      });

      _mapper = config.CreateMapper();
      _fixture.Inject(_mapper);
    }

    [Test]
    public async Task ShouldCreateStudent()
    {
      //Given
      var createStudentRequest = _fixture.Build<CreateStudentRequest>()
        .With(p => p.Gender, Gender.Male)
        .Create();

      DbStudent dbStudent = null;
      _unitOfWorkMock
        .Setup(r => r.StudentRepository.CreateAsync(It.IsAny<DbStudent>()))
        .Callback((DbStudent s) =>
        {
          dbStudent = s;
        })
        .Returns(Task.CompletedTask).Verifiable();

      var sut = CreateSut();

      //When
      await sut.CreateStudentAsync(createStudentRequest);

      //Then
      _unitOfWorkMock.VerifyAll();

      dbStudent.Should().NotBeNull();
      dbStudent.Gender.Should().Be((int)createStudentRequest.Gender);
      dbStudent.FirstName.Should().Be((createStudentRequest.FirstName));
      dbStudent.LastName.Should().Be(createStudentRequest.LastName);
      dbStudent.Patronymic.Should().Be(createStudentRequest.Patronymic);
      dbStudent.UId.Should().Be(createStudentRequest.UId);
    }

    [Test]
    public async Task ShouldUpdateStudent()
    {
      //Given
      var updateStudentRequest = _fixture.Build<UpdateStudentRequest>()
        .With(p => p.Gender, Gender.Male)
        .Create();

      var dbStudent = new DbStudent();

      _unitOfWorkMock
        .Setup(r => r.StudentRepository.UpdateAsync(It.IsAny<DbStudent>()))
        .Callback((DbStudent s) =>
        {
          dbStudent = s;
        })
        .Returns(Task.CompletedTask).Verifiable();

      var sut = CreateSut();

      //When
      await sut.UpdateStudentAsync(updateStudentRequest);

      //Then
      _unitOfWorkMock.VerifyAll();

      dbStudent.Gender.Should().Be((int)updateStudentRequest.Gender);
      dbStudent.FirstName.Should().Be((updateStudentRequest.FirstName));
      dbStudent.LastName.Should().Be(updateStudentRequest.LastName);
      dbStudent.Patronymic.Should().Be(updateStudentRequest.Patronymic);
      dbStudent.UId.Should().Be(updateStudentRequest.UId);
    }

    [Test]
    public async Task ShouldDeleteStudent()
    {
      //Given
      const int studentShouldDeleteId = 1;

      var existingDbStudents = Enumerable.Range(0, 2).Select(x => new DbStudent {Id = x}).ToList();

      _unitOfWorkMock.Setup(r => r.StudentRepository.FindByIdAsync(studentShouldDeleteId))
      .ReturnsAsync(existingDbStudents.FirstOrDefault(x => x.Id == studentShouldDeleteId))
      .Verifiable();

      _unitOfWorkMock
        .Setup(r => r.StudentRepository.RemoveAsync(It.IsAny<DbStudent>()))
        .Callback((DbStudent s) =>
        {
          existingDbStudents.RemoveAll(x => x.Id == s.Id);
        })
        .Returns(Task.CompletedTask).Verifiable();

      var sut = CreateSut();

      //When
      await sut.DeleteStudentAsync(studentShouldDeleteId);

      //Then
      _unitOfWorkMock.VerifyAll();

      var deletedDbStudent = existingDbStudents.FirstOrDefault(x => x.Id == studentShouldDeleteId);
      deletedDbStudent.Should().BeNull();
    }

    [Test]
    public async Task ShouldAddStudentToGroup()
    {
      //Given
      const int studentToAddToGroupId = 1;
      const int groupId = 1;

      var studentGroupModel = _fixture.Create<StudentGroupModel>();

      var existingDbGroups = Enumerable.Range(0, 1).Select(x => new DbGroup
      {
        Id = groupId,
      }).ToList();

      var existingDbStudents = Enumerable.Range(0, 1).Select(x => new DbStudent
      {
        Id = studentToAddToGroupId,
      }).ToList();

      _unitOfWorkMock.Setup(r => r.StudentRepository.GetAsync(It.IsAny<Expression<Func<DbStudent, bool>>>(), It.IsAny<string>()))
        .ReturnsAsync(existingDbStudents.FirstOrDefault(x => x.Id == studentToAddToGroupId))
        .Verifiable();

      _unitOfWorkMock.Setup(r => r.GroupRepository.FindByIdAsync(It.IsAny<long>()))
        .ReturnsAsync(existingDbGroups.FirstOrDefault(x => x.Id == groupId))
        .Verifiable();

      _unitOfWorkMock
        .Setup(r => r.StudentRepository.UpdateAsync(It.IsAny<DbStudent>()))
        .Returns(Task.CompletedTask).Verifiable();

      var sut = CreateSut();

      //When
      await sut.AddStudentToGroupAsync(studentGroupModel);

      //Then
      _unitOfWorkMock.VerifyAll();

      var updatedStudent = existingDbStudents.FirstOrDefault(x => x.Id == studentToAddToGroupId);
      updatedStudent.Should().NotBeNull();
      updatedStudent.Group.Count.Should().BeGreaterThan(0);
    }

    [Test]
    public async Task ShouldDeleteStudentFromGroup()
    {
      //Given
      const int studentToDeleteFromGroupId = 1;
      const int groupId = 1;

      var studentGroupModel = _fixture.Build<StudentGroupModel>()
          .With(x => x.GroupId, groupId)
          .With(x => x.StudentId, studentToDeleteFromGroupId)
          .Create();

      var existingDbGroups = Enumerable.Range(0, 1).Select(x => new DbGroup
      {
        Id = groupId,
      }).ToList();

      var existingDbStudents = Enumerable.Range(0, 1).Select(x => new DbStudent
      {
        Id = studentToDeleteFromGroupId,
        Group = existingDbGroups
      }).ToList();

      _unitOfWorkMock.Setup(r => r.StudentRepository.GetAsync(It.IsAny<Expression<Func<DbStudent, bool>>>(), It.IsAny<string>()))
        .ReturnsAsync(existingDbStudents.FirstOrDefault(x => x.Id == studentToDeleteFromGroupId))
        .Verifiable();

      _unitOfWorkMock
        .Setup(r => r.StudentRepository.UpdateAsync(It.IsAny<DbStudent>()))
        .Returns(Task.CompletedTask).Verifiable();

      var sut = CreateSut();

      //When
      await sut.RemoveStudentFromGroupAsync(studentGroupModel);

      //Then
      _unitOfWorkMock.VerifyAll();

      var updatedStudent = existingDbStudents.FirstOrDefault(x => x.Id == studentToDeleteFromGroupId);
      updatedStudent.Should().NotBeNull();
      updatedStudent.Group.Count.Should().BeLessOrEqualTo(0);
    }

    private StudentsService CreateSut()
    {
      return _fixture.Create<StudentsService>();
    }
  }
}
