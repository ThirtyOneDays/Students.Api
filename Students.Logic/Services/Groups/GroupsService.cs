using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Students.Common.Exceptions;
using Students.Common.Models.UI;
using Students.Logic.Models.Groups;
using Students.Repository.Entities;
using Students.Repository.Interfaces;

namespace Students.Logic.Services.Groups
{
  public class GroupsService : IGroupsService
  {
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GroupsService(IUnitOfWork unitOfWork, IMapper mapper)
    {
      _unitOfWork = unitOfWork;
      _mapper = mapper;
    }

    public async Task CreateGroupAsync(CreateGroupRequest createGroupRequest)
    {
      if (createGroupRequest == null)
        throw new InvalidArgumentException("Create group request model cannot be empty.");

      var dbGroup = _mapper.Map<DbGroup>(createGroupRequest);
      await _unitOfWork.GroupRepository.CreateAsync(dbGroup);
    }

    public async Task DeleteGroupAsync(long groupId)
    {
      var dbGroup = await _unitOfWork.GroupRepository.FindByIdAsync(groupId);
      if (dbGroup == null)
        throw new NotFoundException(typeof(DbGroup), groupId);

      await _unitOfWork.GroupRepository.RemoveAsync(dbGroup);
    }

    public async Task UpdateGroupAsync(UpdateGroupRequest updateGroupRequest)
    {
      if (updateGroupRequest == null)
        throw new InvalidArgumentException("Update group request cannot be empty.");

      var dbGroup = _mapper.Map<DbGroup>(updateGroupRequest);

      try
      {
        await _unitOfWork.GroupRepository.UpdateAsync(dbGroup);
      }
      catch (DbUpdateConcurrencyException)
      {
        throw new NotFoundException(typeof(DbGroup), updateGroupRequest.Id);
      }
    }

    public async Task<GroupModel> GetGroupAsync(long groupId)
    {
      var dbGroup = await _unitOfWork.GroupRepository.FindByIdAsync(groupId);
      if (dbGroup == null)
        throw new NotFoundException(typeof(DbGroup), groupId);

      return _mapper.Map<GroupModel>(dbGroup);
    }

    public async Task<List<GroupModel>> GetGroupsAsync(PagingModel pagingModel)
    {
      var dbGroups = await _unitOfWork.GroupRepository.GetListAsync(pagingModel, includeProperties: nameof(DbGroup.Student));
      return _mapper.Map<List<GroupModel>>(dbGroups);
    }
  }
}
