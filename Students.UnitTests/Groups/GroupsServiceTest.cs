using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using AutoFixture;
using AutoFixture.AutoMoq;
using AutoMapper;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using Students.Logic.Models.Groups;
using Students.Logic.Services.Groups;
using Students.Repository.Entities;
using Students.Repository.Interfaces;

namespace Students.UnitTests.Groups
{
  [TestFixture]
  public class GroupsServiceTest
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
        Assembly.GetAssembly(typeof(GroupsMappingProfile)),
      };

      var config = new MapperConfiguration(cfg =>
      {
        cfg.AddMaps(assemblies);
      });

      _mapper = config.CreateMapper();
      _fixture.Inject(_mapper);
    }

    [Test]
    public async Task ShouldCreateGroup()
    {
      //Given
      var createGroupRequest = _fixture.Create<CreateGroupRequest>();

      DbGroup dbGroup = null;
      _unitOfWorkMock
        .Setup(r => r.GroupRepository.CreateAsync(It.IsAny<DbGroup>()))
        .Callback((DbGroup s) =>
        {
          dbGroup = s;
        })
        .Returns(Task.CompletedTask).Verifiable();

      var sut = CreateSut();

      //When
      await sut.CreateGroupAsync(createGroupRequest);

      //Then
      _unitOfWorkMock.VerifyAll();

      dbGroup.Should().NotBeNull();
      dbGroup.Name.Should().Be(createGroupRequest.Name);
    }

    [Test]
    public async Task ShouldUpdateGroup()
    {
      //Given
      var updateGroupRequest = _fixture.Create<UpdateGroupRequest>();

      var dbGroup = new DbGroup();

      _unitOfWorkMock
        .Setup(r => r.GroupRepository.UpdateAsync(It.IsAny<DbGroup>()))
        .Callback((DbGroup s) =>
        {
          dbGroup = s;
        })
        .Returns(Task.CompletedTask).Verifiable();

      var sut = CreateSut();

      //When
      await sut.UpdateGroupAsync(updateGroupRequest);

      //Then
      _unitOfWorkMock.VerifyAll();

      dbGroup.Name.Should().Be(updateGroupRequest.Name);
    }

    [Test]
    public async Task ShouldDeleteGroup()
    {
      //Given
      const int groupShouldDeleteId = 1;

      var existingDbGroups = Enumerable.Range(0, 2).Select(x => new DbGroup { Id = x }).ToList();

      _unitOfWorkMock.Setup(r => r.GroupRepository.FindByIdAsync(groupShouldDeleteId))
      .ReturnsAsync(existingDbGroups.FirstOrDefault(x => x.Id == groupShouldDeleteId))
      .Verifiable();

      _unitOfWorkMock
        .Setup(r => r.GroupRepository.RemoveAsync(It.IsAny<DbGroup>()))
        .Callback((DbGroup s) =>
        {
          existingDbGroups.RemoveAll(x => x.Id == s.Id);
        })
        .Returns(Task.CompletedTask).Verifiable();

      var sut = CreateSut();

      //When
      await sut.DeleteGroupAsync(groupShouldDeleteId);

      //Then
      _unitOfWorkMock.VerifyAll();

      var deletedDbGroup = existingDbGroups.FirstOrDefault(x => x.Id == groupShouldDeleteId);
      deletedDbGroup.Should().BeNull();
    }

    private GroupsService CreateSut()
    {
      return _fixture.Create<GroupsService>();
    }
  }
}
