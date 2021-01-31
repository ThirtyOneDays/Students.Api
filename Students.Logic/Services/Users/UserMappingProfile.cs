using AutoMapper;
using Students.Logic.Models.Users;
using Students.Repository.Entities;

namespace Students.Logic.Services.Users
{
  public class UserMappingProfile : Profile
  {
    public UserMappingProfile()
    {
      CreateMap<LoginRequest, DbUser>()
          .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.UserName))
          .ForMember(dest => dest.Id, opt => opt.Ignore())
          .ForMember(dest => dest.Password, opt => opt.Ignore());

      CreateMap<DbUser, LoginResponse>()
          .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.Id))
          .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.UserName))
          .ForMember(dest => dest.AccessToken, opt => opt.Ignore())
          .ForMember(dest => dest.RefreshToken, opt => opt.Ignore());
    }
  }
}
