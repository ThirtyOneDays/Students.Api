using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Students.Common.Models.UI;
using Students.Logic.Models.Groups;
using Students.Logic.Services.Groups;

namespace Students.Api.Controllers
{
  [Route("groups")]
  [ApiController]
  public class GroupsController : ControllerBase
  {
    private readonly IGroupsService _groupsService;

    public GroupsController(IGroupsService groupsService)
    {
      _groupsService = groupsService;
    }

    [HttpPost]
    //[Authorize]
    public async Task<IActionResult> CreateGroup(CreateGroupRequest groupRequest)
    {
      await _groupsService.CreateGroupAsync(groupRequest);
      return NoContent();
    }

    [HttpPut]
    //[Authorize]
    public async Task<IActionResult> UpdateGroup(UpdateGroupRequest updateGroupRequest)
    {
      await _groupsService.UpdateGroupAsync(updateGroupRequest);
      return NoContent();
    }

    [HttpDelete]
    //[Authorize]
    public async Task<IActionResult> DeleteGroup(long groupId)
    {
      await _groupsService.DeleteGroupAsync(groupId);
      return NoContent();
    }

    [HttpGet]
    //[Authorize]
    [ProducesResponseType(typeof(GroupModel), 200)]
    public async Task<IActionResult> GetGroup(long groupId)
    {
      var group = await _groupsService.GetGroupAsync(groupId);
      return Ok(group);
    }

    [HttpPost("list")]
    //[Authorize]
    [ProducesResponseType(typeof(List<GroupModel>), 200)]
    public async Task<IActionResult> GetGroups(PagingModel pagingModel)
    {
      var groups = await _groupsService.GetGroupsAsync(pagingModel);
      return Ok(groups);
    }

  }
}
