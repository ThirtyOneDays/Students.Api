using System.Collections.Generic;
using System.Threading.Tasks;
using Students.Common.Models.UI;
using Students.Logic.Models.Groups;

namespace Students.Logic.Services.Groups
{
  public interface IGroupsService
  {
    Task CreateGroupAsync(CreateGroupRequest groupRequest);
    Task DeleteGroupAsync(long groupId);
    Task UpdateGroupAsync(UpdateGroupRequest updateGroupRequest);
    Task<GroupModel> GetGroupAsync(long groupId);
    Task<List<GroupModel>> GetGroupsAsync(PagingModel pagingModel);
  }
}
