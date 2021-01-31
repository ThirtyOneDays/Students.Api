using AutoMapper;
using Students.Logic.Models.Groups;
using Students.Repository.Entities;

namespace Students.Logic.Services.Groups
{
  public class GroupsMappingProfile : Profile
  {
    public GroupsMappingProfile()
    {
      CreateMap<CreateGroupRequest, DbGroup>()
        .ForMember(dest => dest.Id, opt => opt.Ignore())
        .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name));

      CreateMap<UpdateGroupRequest, DbGroup>()
        .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
        .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name));

      CreateMap<DbGroup, GroupModel>()
        .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
        .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
        .ForMember(dest => dest.StudentsCount, opt => opt.MapFrom(src => src.Student.Count));
    }
  }
}
