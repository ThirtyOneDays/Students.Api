using System.Linq;
using AutoMapper;
using Students.Logic.Models.Students;
using Students.Repository.Entities;

namespace Students.Logic.Services.Students
{
  public class StudentsMappingProfile : Profile
  {
    public StudentsMappingProfile()
    {
      CreateMap<CreateStudentRequest, DbStudent>()
        .ForMember(dest => dest.Id, opt => opt.Ignore())
        .ForMember(dest => dest.Gender, opt => opt.MapFrom(src => src.Gender))
        .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.FirstName))
        .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.LastName))
        .ForMember(dest => dest.Patronymic, opt => opt.MapFrom(src => src.Patronymic))
        .ForMember(dest => dest.UId, opt => opt.MapFrom(src => src.UId));

      CreateMap<UpdateStudentRequest, DbStudent>()
        .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
        .ForMember(dest => dest.Gender, opt => opt.MapFrom(src => src.Gender))
        .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.FirstName))
        .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.LastName))
        .ForMember(dest => dest.Patronymic, opt => opt.MapFrom(src => src.Patronymic))
        .ForMember(dest => dest.UId, opt => opt.MapFrom(src => src.UId));

      CreateMap<DbStudent, StudentModel>()
        .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
        .ForMember(dest => dest.Gender, opt => opt.MapFrom(src => src.Gender))
        .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.FirstName))
        .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.LastName))
        .ForMember(dest => dest.Patronymic, opt => opt.MapFrom(src => src.Patronymic))
        .ForMember(dest => dest.UId, opt => opt.MapFrom(src => src.UId))
        .ForMember(dest => dest.Groups, opt => opt.MapFrom(src => string.Join(", ", src.Group.Select(s => s.Name))));
    }
  }
}
